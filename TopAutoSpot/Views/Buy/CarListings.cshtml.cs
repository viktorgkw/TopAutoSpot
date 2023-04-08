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
    /// This class represents the model for the Car Listings page, which displays a list of cars available for purchase.
    /// It is authorized, meaning that only authenticated users can access it.
    /// </summary>
    [Authorize]
    public class CarListingsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly INewsService _newsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CarListingsModel"/> class.
        /// </summary>
        /// <param name="db">The database context.</param>
        /// <param name="newsService">The news service.</param>
        public CarListingsModel(ApplicationDbContext db, INewsService newsService)
        {
            _context = db;
            _newsService = newsService;
        }

        /// <summary>
        /// Gets or sets the order setting for the car listings.
        /// </summary>
        [BindProperty]
        public string? OrderSetting { get; set; }

        /// <summary>
        /// Gets or sets the list of cars to display.
        /// </summary>
        public List<Car> Cars { get; set; } = null!;

        /// <summary>
        /// Gets or sets the list of news articles to display.
        /// </summary>
        public List<Article> News = new();

        /// <summary>
        /// Handles GET requests for the Car Listings page.
        /// Retrieves a list of cars from the database and sorts them according to the order setting.
        /// </summary>
        /// <param name="orderSetting">The order setting for the car listings.</param>
        /// <returns>The page result.</returns>
        public async Task<IActionResult> OnGetAsync(string orderSetting)
        {
            Cars = _context.Cars
                .AsNoTracking()
                .Where(c => c.Status == ListingStatusTypes.Active.ToString() && c.Price > 0)
                .ToList();

            if (orderSetting != null)
            {
                Cars = VehicleCollectionSorter
                    .SortBy(Cars, orderSetting)
                    .ToList();
            }

            News = await _newsService.GetNews(3);

            return Page();
        }

        /// <summary>
        /// Handles POST requests for the Car Listings page.
        /// Redirects the user to the Car Listings page with the updated order setting.
        /// </summary>
        /// <returns>The page result.</returns>
        public IActionResult OnPost()
        {
            return RedirectToPage("/Buy/CarListings", new { orderSetting = OrderSetting });
        }

        /// <summary>
        /// Gets the image data URL for a given car ID.
        /// </summary>
        /// <param name="carId">The ID of the car.</param>
        /// <returns>The image data URL.</returns>
        public string GetImage(string carId)
        {
            byte[] data = _context.VehicleImages
                .AsNoTracking()
                .First(i => i.VehicleId == carId)
                .ImageData;

            string imgDataURL = "data:image;base64," + Convert.ToBase64String(data);
            return imgDataURL;
        }

        /// <summary>
        /// Checks if a car has any images associated with it.
        /// </summary>
        /// <param name="carId">The ID of the car.</param>
        /// <returns>True if the car has images, false otherwise.</returns>
        public bool HasAnyImages(string carId)
        {
            return _context.VehicleImages
                .AsNoTracking()
                .Where(img => img.VehicleId == carId)
                .ToList().Count > 0;
        }

        /// <summary>
        /// Retrieves a list of InterestedListing objects based on the current user's identity.
        /// </summary>
        /// <returns>A list of InterestedListing objects for the current user.</returns>
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
