using NewsAPI.Models;

namespace TopAutoSpot.Services.NewsServices
{
    public interface INewsService
    {
        Task<List<Article>> GetNews(int returnNewsCount);
    }
}