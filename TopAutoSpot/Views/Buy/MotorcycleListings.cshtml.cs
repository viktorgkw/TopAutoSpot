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
    /// Represents a Razor Page for displaying motorcycle listings to buy. Requires authorization to access.
    /// </summary>
    [Authorize]
    public class MotorcycleListingsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly INewsService _newsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="MotorcycleListingsModel"/> class.
        /// </summary>
        /// <param name="db">The application database context.</param>
        /// <param name="newsService">The news service.</param>
        public MotorcycleListingsModel(ApplicationDbContext db, INewsService newsService)
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
        /// Gets or sets the list of motorcycles.
        /// </summary>
        public List<Motorcycle> Motorcycles { get; set; } = null!;

        /// <summary>
        /// Gets or sets the list of articles.
        /// </summary>
        public List<Article> News = new();

        /// <summary>
        /// Called when the page is requested via HTTP GET.
        /// </summary>
        /// <param name="orderSetting">The order setting.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task<IActionResult> OnGetAsync(string orderSetting)
        {
            Motorcycles = _context.Motorcycles
                .AsNoTracking()
                .Where(m => m.Status == ListingStatusTypes.Active.ToString() && m.Price > 0)
                .ToList();

            if (orderSetting != null)
            {
                Motorcycles = VehicleCollectionSorter
                    .SortBy(Motorcycles, orderSetting)
                    .ToList();
            }

            News = await _newsService.GetNews(3);

            return Page();
        }

        /// <summary>
        /// Called when the page is posted back via HTTP POST.
        /// </summary>
        /// <returns>A redirect result.</returns>
        public IActionResult OnPost()
        {
            return RedirectToPage("/Buy/MotorcycleListings", new { orderSetting = OrderSetting });
        }

        /// <summary>
        /// Gets the base64-encoded image data for a motorcycle with the specified ID.
        /// </summary>
        /// <param name="motorcycleId">The ID of the motorcycle.</param>
        /// <returns>The base64-encoded image data.</returns>
        public string GetImage(string motorcycleId)
        {
            byte[] data = _context.VehicleImages
                .AsNoTracking()
                .First(i => i.VehicleId == motorcycleId)
                .ImageData;

            string imgDataURL = "data:image;base64," + Convert.ToBase64String(data);
            return imgDataURL;
        }

        /// <summary>
        /// Determines whether a motorcycle with the specified ID has any images associated with it.
        /// </summary>
        /// <param name="motorcycleId">The ID of the motorcycle.</param>
        /// <returns><c>true</c> if the motorcycle has images; otherwise, <c>false</c>.</returns>
        public bool HasAnyImages(string motorcycleId)
        {
            return _context.VehicleImages
                .AsNoTracking()
                .Where(img => img.VehicleId == motorcycleId)
                .ToList().Count > 0;
        }

        /// <summary>
        /// Gets a list of interested listings for the currently logged-in user.
        /// </summary>
        /// <returns>A list of interested listings.</returns>
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
