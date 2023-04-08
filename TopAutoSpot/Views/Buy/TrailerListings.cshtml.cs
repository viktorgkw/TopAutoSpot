namespace TopAutoSpot.Views.Buy
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;
    using NewsAPI.Models;

    using TopAutoSpot.Data;
    using TopAutoSpot.Data.Models;
    using TopAutoSpot.Data.Models.Enums;
    using TopAutoSpot.Services.NewsServices;
    using TopAutoSpot.Services.Utilities;

    /// <summary>
    /// PageModel class for displaying and sorting active trailer listings, as well as displaying news articles.
    /// </summary>
    [Authorize]
    public class TrailerListingsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly INewsService _newsService;

        public TrailerListingsModel(ApplicationDbContext db, INewsService newsService)
        {
            _context = db;
            _newsService = newsService;
        }

        /// <summary>
        /// Gets or sets the order setting.
        /// </summary>
        [BindProperty]
        public string? OrderSetting { get; set; }

        /// <summary>
        /// List of active trailers to be displayed.
        /// </summary>
        public List<Trailer> Trailers { get; set; } = null!;

        /// <summary>
        /// List of news articles to be displayed.
        /// </summary>
        public List<Article> News = new();

        /// <summary>
        /// Retrieves and sorts active trailer listings and news articles.
        /// </summary>
        public async Task<IActionResult> OnGetAsync(string orderSetting)
        {
            Trailers = _context.Trailers
                .AsNoTracking()
                .Where(t => t.Status == ListingStatusTypes.Active.ToString() && t.Price > 0)
                .ToList();

            if (orderSetting != null)
            {
                Trailers = VehicleCollectionSorter
                    .SortBy(Trailers, orderSetting)
                    .ToList();
            }

            News = await _newsService.GetNews(3);

            return Page();
        }

        /// <summary>
        /// Redirects to the trailer listings page with the specified order setting.
        /// </summary>
        public IActionResult OnPost()
        {
            return RedirectToPage("/Buy/TrailerListings", new { orderSetting = OrderSetting });
        }

        /// <summary>
        /// Retrieves the image for the specified trailer.
        /// </summary>
        /// <param name="trailerId">The ID of the trailer to retrieve the image for.</param>
        /// <returns>The image URL in base64 format.</returns>
        public string GetImage(string trailerId)
        {
            byte[] data = _context.VehicleImages
                .AsNoTracking()
                .First(i => i.VehicleId == trailerId)
                .ImageData;

            string imgDataURL = "data:image;base64," + Convert.ToBase64String(data);
            return imgDataURL;
        }

        /// <summary>
        /// Checks if there are any images for the specified trailer.
        /// </summary>
        /// <param name="trailerId">The ID of the trailer to check for images.</param>
        /// <returns>True if there are any images, false otherwise.</returns>
        public bool HasAnyImages(string trailerId)
        {
            return _context.VehicleImages
                .AsNoTracking()
                .Where(img => img.VehicleId == trailerId)
                .ToList().Count > 0;
        }

        /// <summary>
        /// Retrieves a list of interested listings for the current user.
        /// </summary>
        /// <returns>A list of interested listings for the current user.</returns>
        public List<InterestedListing> GetInterestedVehicles()
        {
            User currentUser = _context.Users
                .AsNoTracking()
                .First(u => u.UserName == User.Identity!.Name);

            return _context.InterestedInListings
                .AsNoTracking()
                .Where(l => l.UserId == currentUser.Id)
                .ToList();
        }
    }
}
