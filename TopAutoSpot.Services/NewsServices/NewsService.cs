namespace TopAutoSpot.Services.NewsServices
{
    using Microsoft.Extensions.Configuration;
    using NewsAPI;
    using NewsAPI.Constants;
    using NewsAPI.Models;

    /// <summary>
    /// This class is a service class for news.
    /// </summary>
    public class NewsService : INewsService
    {
        private readonly IConfiguration _configuration;

        public NewsService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// This method returns specific amount of news retrieved via the News API.
        /// </summary>
        /// <param name="returnNewsCount">Number of news that you want to retrieve.</param>
        /// <returns>List of retrieved Articles.</returns>
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

        /// <summary>
        /// This method ensures that each Article will be unique and there will be no repeated ones.
        /// </summary>
        /// <param name="articlesResponse">List of all Articles</param>
        /// <param name="returnCount">Number of news that you want to retrieve.</param>
        /// <returns>List of retrieved unique Articles.</returns>
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
