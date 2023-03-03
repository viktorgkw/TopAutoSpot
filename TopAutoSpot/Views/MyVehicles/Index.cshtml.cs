using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NewsAPI.Models;
using TopAutoSpot.Data;
using TopAutoSpot.Data.Models;
using TopAutoSpot.Services.NewsServices;

namespace TopAutoSpot.Views.MyVehicles
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
        public List<Auction> Auctions { get; private set; }

        public int OverallCount { get; private set; }

        public async Task<IActionResult> OnGetAsync()
        {
            User currentUser = _context.Users
                .AsNoTracking()
                .First(u => u.UserName == User.Identity.Name);

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

        public string GetImage(string carId)
        {
            byte[] data = _context.VehicleImages
                .AsNoTracking()
                .First(i => i.VehicleId == carId)
                .ImageData;

            string imgDataURL = "data:image;base64," + Convert.ToBase64String(data);
            return imgDataURL;
        }

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

        public bool HasAnyImages(string carId)
        {
            return _context.VehicleImages
                .AsNoTracking()
                .Where(img => img.VehicleId == carId)
                .ToList().Count > 0;
        }
    }
}
