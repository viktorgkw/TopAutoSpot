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
    /// A model for the page that displays a list of bus listings.
    /// </summary>
    [Authorize]
    public class BusListingsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly INewsService _newsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="BusListingsModel"/> class.
        /// </summary>
        /// <param name="db">The application database context.</param>
        /// <param name="newsService">The news service.</param>
        public BusListingsModel(ApplicationDbContext db, INewsService newsService)
        {
            _context = db;
            _newsService = newsService;
        }

        /// <summary>
        /// Gets or sets the order setting for the vehicle collection sorter.
        /// </summary>
        [BindProperty]
        public string? OrderSetting { get; set; }

        /// <summary>
        /// Gets or sets the list of buses to display.
        /// </summary>
        public List<Bus> Buses { get; set; } = null!;

        /// <summary>
        /// Gets or sets the list of news articles to display.
        /// </summary>
        public List<Article> News = new();

        /// <summary>
        /// Handles HTTP GET requests for the page.
        /// </summary>
        /// <param name="orderSetting">The order setting for the vehicle collection sorter.</param>
        /// <returns>A task that represents the asynchronous GET operation.</returns>
        public async Task<IActionResult> OnGetAsync(string orderSetting)
        {
            Buses = _context.Buses
                .AsNoTracking()
                .Where(b => b.Status == ListingStatusTypes.Active.ToString() && b.Price > 0)
                .ToList();

            if (orderSetting != null)
            {
                Buses = VehicleCollectionSorter
                    .SortBy(Buses, orderSetting)
                    .ToList();
            }

            News = await _newsService.GetNews(3);

            return Page();
        }

        /// <summary>
        /// Handles HTTP POST requests for the page.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> that represents the result of the POST operation.</returns>
        public IActionResult OnPost()
        {
            return RedirectToPage("/Buy/BusListings", new { orderSetting = OrderSetting });
        }

        /// <summary>
        /// Gets the image data URL for the specified bus.
        /// </summary>
        /// <param name="busId">The ID of the bus.</param>
        /// <returns>The image data URL.</returns>
        public string GetImage(string busId)
        {
            byte[] data = _context.VehicleImages
                .AsNoTracking()
                .First(i => i.VehicleId == busId)
                .ImageData;

            string imgDataURL = "data:image;base64," + Convert.ToBase64String(data);
            return imgDataURL;
        }

        /// <summary>
        /// Determines whether the specified bus has any images.
        /// </summary>
        /// <param name="busId">The ID of the bus.</param>
        /// <returns>true if the bus has any images; otherwise, false.</returns>
        public bool HasAnyImages(string busId)
        {
            return _context.VehicleImages
                .AsNoTracking()
                .Where(img => img.VehicleId == busId)
                .ToList().Count > 0;
        }

        /// <summary>
        /// Gets the list of interested listings for the current user.
        /// </summary>
        /// <returns>The list of interested listings.</returns>
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
