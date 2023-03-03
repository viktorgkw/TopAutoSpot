using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TopAutoSpot.Data;
using TopAutoSpot.Models;

namespace TopAutoSpot.Views.AdministratorViews
{
    [Authorize]
    public class ManageListingsModel : PageModel
    {
        public ApplicationDbContext _context;
        public ManageListingsModel(ApplicationDbContext db)
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

        public IActionResult OnGet()
        {
            if (User.IsInRole("Administrator"))
            {
                Boats = _context.Boats
                    .AsNoTracking()
                    .ToList();

                Buses = _context.Buses
                    .AsNoTracking()
                    .ToList();

                Cars = _context.Cars
                    .AsNoTracking()
                    .ToList();

                Motorcycles = _context.Motorcycles
                    .AsNoTracking()
                    .ToList();

                Trailers = _context.Trailers
                    .AsNoTracking()
                    .ToList();

                Trucks = _context.Trucks
                    .AsNoTracking()
                    .ToList();

                OverallCount = Boats.Count + Buses.Count + Cars.Count +
                        Motorcycles.Count + Trailers.Count + Trucks.Count;

                return Page();
            }

            return RedirectToPage("/NotFound");
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

        public bool HasAnyImages(string carId)
        {
            return _context.VehicleImages
                .AsNoTracking()
                .Where(img => img.VehicleId == carId)
                .ToList().Count > 0;
        }
    }
}
