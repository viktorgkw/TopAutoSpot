using NewsAPI.Models;
using TopAutoSpot.Services.NewsServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;

namespace TopAutoSpot.Views.Auction
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private INewsService _newsService;
        public IndexModel(INewsService newsService)
        {
            _newsService = newsService;
        }

        public List<Article> News = new List<Article>();

        public async Task<IActionResult> OnGetAsync()
        {
            News = await _newsService.GetNews(3);

            return Page();
        }
    }
}
