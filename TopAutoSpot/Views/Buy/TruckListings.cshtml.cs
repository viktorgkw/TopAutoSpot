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
    /// The TruckListingsModel class is a PageModel that represents a page with a list of available trucks for sale.
    /// It provides methods for getting truck images, checking if a truck has any images, and retrieving a list of vehicles that the current user is interested in.
    /// The class is decorated with the [Authorize] attribute, which requires the user to be authenticated to access the page.
    /// </summary>
    [Authorize]
    public class TruckListingsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly INewsService _newsService;

        public TruckListingsModel(ApplicationDbContext db, INewsService newsService)
        {
            _context = db;
            _newsService = newsService;
        }

        /// <summary>
        /// BindProperty for OrderSetting, which is used to set the order in which the trucks are displayed.
        /// </summary>
        [BindProperty]
        public string? OrderSetting { get; set; }

        /// <summary>
        /// List of trucks to display on the page.
        /// </summary>
        public List<Truck> Trucks { get; set; } = null!;

        /// <summary>
        /// List of news articles to display on the page.
        /// </summary>
        public List<Article> News = new();

        /// <summary>
        /// Retrieves a list of trucks from the database and sorts them according to the OrderSetting parameter.
        /// </summary>
        /// <param name="orderSetting">The order in which the trucks should be displayed.</param>
        /// <returns>An IActionResult representing the page.</returns>
        public async Task<IActionResult> OnGetAsync(string orderSetting)
        {
            Trucks = _context.Trucks
                .AsNoTracking()
                .Where(t => t.Status == ListingStatusTypes.Active.ToString() && t.Price > 0)
                .ToList();

            if (orderSetting != null)
            {
                Trucks = VehicleCollectionSorter
                    .SortBy(Trucks, orderSetting)
                    .ToList();
            }

            News = await _newsService.GetNews(3);

            return Page();
        }

        /// <summary>
        /// Redirects the user to the truck listings page with the OrderSetting parameter set.
        /// </summary>
        /// <returns>An IActionResult representing the page.</returns>
        public IActionResult OnPost()
        {
            return RedirectToPage("/Buy/TruckListings", new { orderSetting = OrderSetting });
        }

        /// <summary>
        /// Retrieves the image for a truck.
        /// </summary>
        /// <param name="truckId">The ID of the truck to retrieve the image for.</param>
        /// <returns>A string representing the image data URL.</returns>
        public string GetImage(string truckId)
        {
            byte[] data = _context.VehicleImages
                .AsNoTracking()
                .First(i => i.VehicleId == truckId)
                .ImageData;

            string imgDataURL = "data:image;base64," + Convert.ToBase64String(data);
            return imgDataURL;
        }

        /// <summary>
        /// Determines whether a truck has any images.
        /// </summary>
        /// <param name="truckId">The ID of the truck to check.</param>
        /// <returns>A boolean indicating whether the truck has any images.</returns>
        public bool HasAnyImages(string truckId)
        {
            return _context.VehicleImages
                .AsNoTracking()
                .Where(img => img.VehicleId == truckId)
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
