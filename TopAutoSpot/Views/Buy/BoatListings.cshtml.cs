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
    /// Page model for Boat Listings page.
    /// </summary>
    [Authorize]
    public class BoatListingsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly INewsService _newsService;

        public BoatListingsModel(ApplicationDbContext db, INewsService newsService)
        {
            _context = db;
            _newsService = newsService;
        }

        /// <summary>
        /// Gets or sets the OrderSetting parameter from the query string.
        /// </summary>
        [BindProperty]
        public string? OrderSetting { get; set; }

        /// <summary>
        /// Gets or sets the list of boats to be displayed.
        /// </summary>
        public List<Boat> Boats { get; set; } = null!;

        /// <summary>
        /// Gets or sets the list of news articles to be displayed.
        /// </summary>
        public List<Article> News = new();

        /// <summary>
        /// Gets the list of boats to be displayed based on the query parameters.
        /// </summary>
        /// <param name="orderSetting">The order setting to sort the boats by.</param>
        /// <returns>The page to be displayed.</returns>
        public async Task<IActionResult> OnGetAsync(string orderSetting)
        {
            Boats = _context.Boats
                .AsNoTracking()
                .Where(b => b.Status == ListingStatusTypes.Active.ToString() && b.Price > 0)
                .ToList();

            if (orderSetting != null)
            {
                Boats = VehicleCollectionSorter
                    .SortBy(Boats, orderSetting)
                    .ToList();
            }

            News = await _newsService.GetNews(3);

            return Page();
        }

        /// <summary>
        /// Redirects to the Boat Listings page with the updated order setting.
        /// </summary>
        /// <returns>The redirected page.</returns>
        public IActionResult OnPost()
        {
            return RedirectToPage("/Buy/BoatListings", new { orderSetting = OrderSetting });
        }

        /// <summary>
        /// Gets the image data URL for a given boat ID.
        /// </summary>
        /// <param name="boatId">The ID of the boat.</param>
        /// <returns>The image data URL for the boat image.</returns>
        public string GetImage(string boatId)
        {
            byte[] data = _context.VehicleImages
                .AsNoTracking()
                .First(i => i.VehicleId == boatId)
                .ImageData;

            string imgDataURL = "data:image;base64," + Convert.ToBase64String(data);
            return imgDataURL;
        }

        /// <summary>
        /// Checks if a given boat has any associated images.
        /// </summary>
        /// <param name="boatId">The ID of the boat.</param>
        /// <returns>True if the boat has images, false otherwise.</returns>
        public bool HasAnyImages(string boatId)
        {
            return _context.VehicleImages
                .AsNoTracking()
                .Where(img => img.VehicleId == boatId)
                .ToList().Count > 0;
        }

        /// <summary>
        /// Gets the list of interested listings for the current user.
        /// </summary>
        /// <returns>The list of interested listings for the user.</returns>
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
