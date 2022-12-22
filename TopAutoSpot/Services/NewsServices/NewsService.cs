﻿using NewsAPI;
using NewsAPI.Models;
using NewsAPI.Constants;

namespace TopAutoSpot.Services.NewsServices
{
    public class NewsService : INewsService
    {
        public async Task<List<Article>> GetNews(int returnNewsCount)
        {
            List<Article> articlesResult = new List<Article>();

            var newsApiClient = new NewsApiClient("YOUR_KEY");
            var articlesResponse = await newsApiClient.GetEverythingAsync(new EverythingRequest
            {
                Q = "Vehicle"
            });

            if (articlesResponse.Status == Statuses.Ok)
            {
                articlesResult = articlesResponse.Articles.Take(returnNewsCount).ToList();

                return articlesResult;
            }

            return null;
        }
    }
}
