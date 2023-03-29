namespace TopAutoSpot.Services.NewsServices
{
    using NewsAPI;
    using NewsAPI.Constants;
    using NewsAPI.Models;

    public class NewsService : INewsService
    {
        private readonly IConfiguration _configuration;

        public NewsService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<Article>> GetNews(int returnNewsCount)
        {
            NewsApiClient newsApiClient = new NewsApiClient(_configuration["NewsAPI:Key"]);
            ArticlesResult articlesResponse = await newsApiClient.GetEverythingAsync(new EverythingRequest
            {
                Q = "Vehicles"
            });

            if (articlesResponse.Status == Statuses.Ok)
            {
                if (articlesResponse.Articles.Count > returnNewsCount)
                {
                    return GetRandomArticles(articlesResponse, returnNewsCount);
                }
                else if (articlesResponse.Articles.Count > 0)
                {
                    return articlesResponse.Articles;
                }
                else
                {
                    return null;
                }
            }

            return null;
        }

        private List<Article> GetRandomArticles(ArticlesResult articlesResponse, int returnCount)
        {
            Dictionary<int, string> articlesTitles = new Dictionary<int, string>();
            Random random = new Random();

            while (articlesTitles.Count != returnCount)
            {
                int currentRandom = random.Next(0, articlesResponse.Articles.Count);

                if (!articlesTitles.ContainsKey(currentRandom))
                {
                    articlesTitles.Add(currentRandom, articlesResponse.Articles[currentRandom].Title);
                }
            }

            List<Article> foundArticles = new List<Article>();

            foreach (KeyValuePair<int, string> articleTitle in articlesTitles)
            {
                foundArticles.Add(articlesResponse.Articles.First(a => a.Title == articleTitle.Value));
            }

            return foundArticles;
        }
    }
}
