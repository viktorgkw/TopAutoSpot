namespace TopAutoSpot.Services.NewsServices
{
    using Microsoft.Extensions.Configuration;
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
            NewsApiClient newsApiClient = new(_configuration["NewsAPI:Key"]);
            ArticlesResult articlesResponse = await newsApiClient
                .GetEverythingAsync(new EverythingRequest { Q = "Vehicles" });

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
                    return new List<Article>();
                }
            }

            return new List<Article>();
        }

        private static List<Article> GetRandomArticles(ArticlesResult articlesResponse, int returnCount)
        {
            Dictionary<int, string> articlesTitles = new();
            Random random = new();

            while (articlesTitles.Count != returnCount)
            {
                int currentRandom = random.Next(0, articlesResponse.Articles.Count);

                if (!articlesTitles.ContainsKey(currentRandom))
                {
                    articlesTitles.Add(currentRandom, articlesResponse.Articles[currentRandom].Title);
                }
            }

            List<Article> foundArticles = new();

            foreach (KeyValuePair<int, string> articleTitle in articlesTitles)
            {
                foundArticles.Add(articlesResponse.Articles.First(a => a.Title == articleTitle.Value));
            }

            return foundArticles;
        }
    }
}
