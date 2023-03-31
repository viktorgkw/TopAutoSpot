using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NewsAPI.Models;
using TopAutoSpot.Services.NewsServices;

namespace TopAutoSpot.Views
{
    public class IndexModel : PageModel
    {
        private readonly INewsService _newsService;

        public IndexModel(INewsService newsService)
        {
            _newsService = newsService;
        }

        public List<Article> News = new();

        public async Task<IActionResult> OnGetAsync()
        {
            News = await _newsService.GetNews(6);

            return Page();
        }
    }
}
