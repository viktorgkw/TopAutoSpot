using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NewsAPI.Models;
using TopAutoSpot.Data;
using TopAutoSpot.Models;
using TopAutoSpot.Models.Utilities;
using TopAutoSpot.Services.NewsServices;

namespace TopAutoSpot.Views.Trade
{
    [Authorize]
    public class IndexModel : PageModel
    {
        public ApplicationDbContext _context;
        private INewsService _newsService;
        public IndexModel(ApplicationDbContext db, INewsService newsService)
        {
            _context = db;
            _newsService = newsService;
        }

        public List<Article> News = new List<Article>();

        public List<Boat> Boats { get; private set; }
        public List<Bus> Buses { get; private set; }
        public List<Car> Cars { get; private set; }
        public List<Motorcycle> Motorcycles { get; private set; }
        public List<Trailer> Trailers { get; private set; }
        public List<Truck> Trucks { get; private set; }
        public int OverallCount { get; private set; }

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

        public string GetImage(string carId)
        {
            byte[] data = _context.VehicleImages
                .AsNoTracking()
                .First(i => i.VehicleId == carId).ImageData;

            string imgDataURL = "data:image;base64," + Convert.ToBase64String(data);
            return imgDataURL;
        }

        public bool HasAnyImages(string carId)
        {
            return _context.VehicleImages
                .AsNoTracking()
                .Where(img => img.VehicleId == carId)
                .ToList().Count > 0;
        }
    }
}
