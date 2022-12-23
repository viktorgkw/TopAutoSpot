using NewsAPI;
using NewsAPI.Models;
using NewsAPI.Constants;

namespace TopAutoSpot.Services.NewsServices
{
    public class NewsService : INewsService
    {
        private readonly IConfiguration _configuration;
        public NewsService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<Article>> GetNews(int returnNewsCount)
        {
            List<Article> articlesResult = new List<Article>();

            var newsApiClient = new NewsApiClient(_configuration["NewsAPI:Key"]);
            var articlesResponse = await newsApiClient.GetEverythingAsync(new EverythingRequest
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

            for (int i = 0; i < returnCount; i++)
            {
                var currentRandom = random.Next(0, articlesResponse.Articles.Count + 1);

                if (!articlesTitles.ContainsKey(currentRandom))
                {
                    articlesTitles.Add(currentRandom, articlesResponse.Articles[currentRandom].Title);
                }
                else
                {
                    i--;
                }
            }

            var foundArticles = new List<Article>();

            foreach (var articleTitle in articlesTitles)
            {
                foundArticles.Add(articlesResponse.Articles.First(a => a.Title == articleTitle.Value));
            }

            return foundArticles;
        }
    }
}
