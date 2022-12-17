using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TopAutoSpot.Data;
using TopAutoSpot.Data.Entities;

namespace TopAutoSpot.Views.MyVehicles
{
    [Authorize]
    public class IndexModel : PageModel
    {
        public ApplicationDbContext _context;
        public IndexModel(ApplicationDbContext db)
        {
            _context = db;
        }

        public List<Boat> Boats { get; private set; }
        public List<Bus> Buses { get; private set; }
        public List<Car> Cars { get; private set; }
        public List<Motorcycle> Motorcycles { get; private set; }
        public List<Trailer> Trailers { get; private set; }
        public List<Truck> Trucks { get; private set; }

        public int OverallCount { get; private set; }

        public async Task OnGetAsync()
        {
            var currentUser = _context.Users
                .First(u => u.UserName == User.Identity.Name);

            Boats = await _context.Boats
                    .Where(boat => boat.CreatedBy == currentUser.Id)
                    .ToListAsync();

            Buses = await _context.Buses
                    .Where(bus => bus.CreatedBy == currentUser.Id)
                    .ToListAsync();

            Cars = await _context.Cars
                    .Where(car => car.CreatedBy == currentUser.Id)
                    .ToListAsync();

            Motorcycles = await _context.Motorcycles
                    .Where(motorcycle => motorcycle.CreatedBy == currentUser.Id)
                    .ToListAsync();

            Trailers = await _context.Trailers
                    .Where(trailer => trailer.CreatedBy == currentUser.Id)
                    .ToListAsync();

            Trucks = await _context.Trucks
                    .Where(truck => truck.CreatedBy == currentUser.Id)
                    .ToListAsync();

            OverallCount = Boats.Count + Buses.Count + Cars.Count +
                    Motorcycles.Count + Trailers.Count + Trucks.Count;
        }

        public string GetImage(string carId)
        {
            var data = _context.VehicleImages.First(i => i.VehicleId == carId).ImageData;
            string imgDataURL = "data:image;base64," + Convert.ToBase64String(data);
            return imgDataURL;
        }

        public bool HasAnyImages(string carId)
        {
            return _context.VehicleImages.Where(img => img.VehicleId == carId).ToList().Count > 0;
        }
    }
}
