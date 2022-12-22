using TopAutoSpot.Data;
using TopAutoSpot.Models;
using TopAutoSpot.Models.Utilities;
using TopAutoSpot.Services.NewsServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using NewsAPI.Models;

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
            Boats = await _context.Boats
                    .Where(boat => boat.Price == 0 && boat.Status == StatusTypes.Active.ToString())
                    .ToListAsync();

            Buses = await _context.Buses
                    .Where(bus => bus.Price == 0 && bus.Status == StatusTypes.Active.ToString())
                    .ToListAsync();

            Cars = await _context.Cars
                    .Where(car => car.Price == 0 && car.Status == StatusTypes.Active.ToString())
                    .ToListAsync();

            Motorcycles = await _context.Motorcycles
                    .Where(motorcycle => motorcycle.Price == 0 && motorcycle.Status == StatusTypes.Active.ToString())
                    .ToListAsync();

            Trailers = await _context.Trailers
                    .Where(trailer => trailer.Price == 0 && trailer.Status == StatusTypes.Active.ToString())
                    .ToListAsync();

            Trucks = await _context.Trucks
                    .Where(truck => truck.Price == 0 && truck.Status == StatusTypes.Active.ToString())
                    .ToListAsync();

            OverallCount = Boats.Count + Buses.Count + Cars.Count +
                    Motorcycles.Count + Trailers.Count + Trucks.Count;

            News = await _newsService.GetNews(3);

            return Page();
        }

        public string GetImage(string carId)
        {
            var data = _context.VehicleImages
                .First(i => i.VehicleId == carId).ImageData;

            string imgDataURL = "data:image;base64," + Convert.ToBase64String(data);
            return imgDataURL;
        }

        public bool HasAnyImages(string carId)
        {
            return _context.VehicleImages
                .Where(img => img.VehicleId == carId)
                .ToList().Count > 0;
        }
    }
}
