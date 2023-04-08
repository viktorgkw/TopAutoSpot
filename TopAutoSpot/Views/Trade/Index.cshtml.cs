namespace TopAutoSpot.Views.Trade
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

    /// <summary>
    /// Represents the page model for the Trading Index page.
    /// </summary>
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        private readonly INewsService _newsService;

        public IndexModel(ApplicationDbContext db, INewsService newsService)
        {
            _context = db;
            _newsService = newsService;
        }

        public List<Article> News = new();

        public List<Boat> Boats { get; private set; } = null!;

        public List<Bus> Buses { get; private set; } = null!;

        public List<Car> Cars { get; private set; } = null!;

        public List<Motorcycle> Motorcycles { get; private set; } = null!;

        public List<Trailer> Trailers { get; private set; } = null!;

        public List<Truck> Trucks { get; private set; } = null!;

        public int OverallCount { get; private set; }

        /// <summary>
        /// This method handles the HTTP GET request for the Index page, retrieving all the vehicles with a price of zero and a status of "Active"
        /// from each vehicle category (boats, buses, cars, motorcycles, trailers, and trucks) from the database context, and storing them in
        /// corresponding lists. It also calculates the overall count of such vehicles. Additionally, it retrieves the latest three news articles
        /// using the injected INewsService, and returns the Index page.
        /// </summary>
        /// <returns>An IActionResult representing the Index page.</returns>
        public async Task<IActionResult> OnGetAsync()
        {
            Boats = _context.Boats
                .AsNoTracking()
                .Where(boat => boat.Price == 0 && boat.Status == ListingStatusTypes.Active.ToString())
                .ToList();

            Buses = _context.Buses
                .AsNoTracking()
                .Where(bus => bus.Price == 0 && bus.Status == ListingStatusTypes.Active.ToString())
                .ToList();

            Cars = _context.Cars
                .AsNoTracking()
                .Where(car => car.Price == 0 && car.Status == ListingStatusTypes.Active.ToString())
                .ToList();

            Motorcycles = _context.Motorcycles
                .AsNoTracking()
                .Where(motorcycle => motorcycle.Price == 0 && motorcycle.Status == ListingStatusTypes.Active.ToString())
                .ToList();

            Trailers = _context.Trailers
                .AsNoTracking()
                .Where(trailer => trailer.Price == 0 && trailer.Status == ListingStatusTypes.Active.ToString())
                .ToList();

            Trucks = _context.Trucks
                .AsNoTracking()
                .Where(truck => truck.Price == 0 && truck.Status == ListingStatusTypes.Active.ToString())
                .ToList();

            OverallCount = Boats.Count + Buses.Count + Cars.Count +
                    Motorcycles.Count + Trailers.Count + Trucks.Count;

            News = await _newsService.GetNews(3);

            return Page();
        }

        /// <summary>
        /// Gets the data URL of the main image of the vehicle.
        /// </summary>
        /// <returns>The data URL of the main image of the vehicle.</returns>
        public string GetImage(string carId)
        {
            byte[] data = _context.VehicleImages
                .AsNoTracking()
                .First(i => i.VehicleId == carId).ImageData;

            string imgDataURL = "data:image;base64," + Convert.ToBase64String(data);
            return imgDataURL;
        }

        /// <summary>
        /// Checks if the vehicle has any images.
        /// </summary>
        /// <returns>True if the vehicle has any images, false otherwise.</returns>
        public bool HasAnyImages(string carId)
        {
            return _context.VehicleImages
                .AsNoTracking()
                .Where(img => img.VehicleId == carId)
                .ToList().Count > 0;
        }
    }
}
