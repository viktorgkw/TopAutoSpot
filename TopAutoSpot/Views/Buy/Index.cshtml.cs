namespace TopAutoSpot.Views.Buy
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using NewsAPI.Models;

    using TopAutoSpot.Services.NewsServices;

    /// <summary>
    /// This page model represents the index page of the application, which is the default landing page.
    /// </summary>
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly INewsService _newsService;

        /// <summary>
        /// Constructs an instance of the IndexModel with the specified INewsService.
        /// </summary>
        /// <param name="newsService">The INewsService implementation to use for fetching news.</param>
        public IndexModel(INewsService newsService)
        {
            _newsService = newsService;
        }

        /// <summary>
        /// Gets or sets the list of news articles to display on the index page.
        /// </summary>
        public List<Article> News = new();

        /// <summary>
        /// Handles the GET request to the index page.
        /// </summary>
        /// <returns>A Task representing the asynchronous operation.</returns>
        public async Task<IActionResult> OnGetAsync()
        {
            News = await _newsService.GetNews(3);

            return Page();
        }
    }
}
