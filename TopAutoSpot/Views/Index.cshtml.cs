namespace TopAutoSpot.Views
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using NewsAPI.Models;
    using TopAutoSpot.Services.NewsServices;

    /// <summary>
    /// Represents the page model for the Index page.
    /// </summary>
    public class IndexModel : PageModel
    {
        private readonly INewsService _newsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="IndexModel"/> class.
        /// </summary>
        /// <param name="newsService">The news service to use.</param>
        public IndexModel(INewsService newsService)
        {
            _newsService = newsService;
        }

        /// <summary>
        /// Gets or sets the list of news articles.
        /// </summary>
        public List<Article> News = new();

        /// <summary>
        /// Handles GET requests for the index page.
        /// </summary>
        /// <returns>The page result.</returns>
        public async Task<IActionResult> OnGetAsync()
        {
            News = await _newsService.GetNews(6);

            return Page();
        }
    }
}
