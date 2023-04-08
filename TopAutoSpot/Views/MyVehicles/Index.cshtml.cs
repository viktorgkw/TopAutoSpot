namespace TopAutoSpot.Views.MyVehicles
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;
    using NewsAPI.Models;

    using TopAutoSpot.Data;
    using TopAutoSpot.Data.Models;
    using TopAutoSpot.Services.NewsServices;

    /// <summary>
    /// Page model for the Index page that displays a list of articles and vehicle listings.
    /// </summary>
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        private readonly INewsService _newsService;

        /// <summary>
        /// Initializes a new instance of the IndexModel class with the given database context and news service.
        /// </summary>
        /// <param name="db">The application database context.</param>
        /// <param name="newsService">The news service.</param>
        public IndexModel(ApplicationDbContext db, INewsService newsService)
        {
            _context = db;
            _newsService = newsService;
        }

        /// <summary>
        /// Gets or sets the list of news articles to display on the Index page.
        /// </summary>
        public List<Article> News = new();

        /// <summary>
        /// Gets or sets the list of boats to display on the Index page.
        /// </summary>
        public List<Boat> Boats { get; private set; } = null!;

        /// <summary>
        /// Gets or sets the list of buses to display on the Index page.
        /// </summary>
        public List<Bus> Buses { get; private set; } = null!;

        /// <summary>
        /// Gets or sets the list of cars to display on the Index page.
        /// </summary>
        public List<Car> Cars { get; private set; } = null!;

        /// <summary>
        /// Gets or sets the list of motorcycles to display on the Index page.
        /// </summary>
        public List<Motorcycle> Motorcycles { get; private set; } = null!;

        /// <summary>
        /// Gets or sets the list of trailers to display on the Index page.
        /// </summary>
        public List<Trailer> Trailers { get; private set; } = null!;

        /// <summary>
        /// Gets or sets the list of trucks to display on the Index page.
        /// </summary>
        public List<Truck> Trucks { get; private set; } = null!;

        /// <summary>
        /// Gets or sets the list of auctions to display on the Index page.
        /// </summary>
        public List<Auction> Auctions { get; private set; } = null!;

        /// <summary>
        /// Gets or sets the overall count of all listings to display on the Index page.
        /// </summary>
        public int OverallCount { get; private set; }

        /// <summary>
        /// Handles the HTTP GET request for the Index page.
        /// </summary>
        /// <returns>The IActionResult for the page.</returns>
        public async Task<IActionResult> OnGetAsync()
        {
            User currentUser = _context.Users
                .AsNoTracking()
                .First(u => u.UserName == User.Identity!.Name);

            Boats = _context.Boats
                .AsNoTracking()
                .Where(boat => boat.CreatedBy == currentUser.Id)
                .ToList();

            Buses = _context.Buses
                .AsNoTracking()
                .Where(bus => bus.CreatedBy == currentUser.Id)
                .ToList();

            Cars = _context.Cars
                .AsNoTracking()
                .Where(car => car.CreatedBy == currentUser.Id)
                .ToList();

            Motorcycles = _context.Motorcycles
                .AsNoTracking()
                .Where(motorcycle => motorcycle.CreatedBy == currentUser.Id)
                .ToList();

            Trailers = _context.Trailers
                .AsNoTracking()
                .Where(trailer => trailer.CreatedBy == currentUser.Id)
                .ToList();

            Trucks = _context.Trucks
                .AsNoTracking()
                .Where(truck => truck.CreatedBy == currentUser.Id)
                .ToList();

            OverallCount = Boats.Count + Buses.Count + Cars.Count +
                    Motorcycles.Count + Trailers.Count + Trucks.Count;

            Auctions = _context.Auctions
                .AsNoTracking()
                .Where(auction => auction.AuctioneerId == currentUser.Id)
                .ToList();

            News = await _newsService.GetNews(3);

            return Page();
        }

        /// <summary>
        /// Retrieves the image data in base64 format for a given car ID.
        /// </summary>
        /// <param name="carId">The ID of the car to retrieve the image for.</param>
        /// <returns>The image data in base64 format.</returns>
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
        /// Retrieves the image data in base64 format for a given auction ID.
        /// </summary>
        /// <param name="auctionId">The ID of the auction to retrieve the image for.</param>
        /// <returns>The image data in base64 format.</returns>
        public string GetAuctionImage(string auctionId)
        {
            string carId = _context.Auctions
                .AsNoTracking()
                .First(a => a.Id == auctionId)
                .VehicleId;

            byte[] data = _context.VehicleImages
                .AsNoTracking()
                .First(i => i.VehicleId == carId)
                .ImageData;

            string imgDataURL = "data:image;base64," + Convert.ToBase64String(data);
            return imgDataURL;
        }

        /// <summary>
        /// Determines if a given car ID has any images associated with it.
        /// </summary>
        /// <param name="carId">The ID of the car to check for images.</param>
        /// <returns>True if the car has at least one image, false otherwise.</returns>
        public bool HasAnyImages(string carId)
        {
            return _context.VehicleImages
                .AsNoTracking()
                .Where(img => img.VehicleId == carId)
                .ToList().Count > 0;
        }
    }
}
