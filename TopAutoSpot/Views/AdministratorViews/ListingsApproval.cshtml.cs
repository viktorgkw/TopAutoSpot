using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TopAutoSpot.Data;
using TopAutoSpot.Models;
using TopAutoSpot.Models.Utilities;

namespace TopAutoSpot.Views.AdministratorViews
{
    [Authorize]
    public class ListingsApprovalModel : PageModel
    {
        private ApplicationDbContext _context;

        public ListingsApprovalModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public int OverallCount { get; private set; }
        public List<Car> Cars { get; set; }
        public List<Motorcycle> Motorcycles { get; set; }
        public List<Truck> Trucks { get; set; }
        public List<Trailer> Trailers { get; set; }
        public List<Boat> Boats { get; set; }
        public List<Bus> Buses { get; set; }

        public IActionResult OnGet()
        {
            if (User.IsInRole("Administrator"))
            {
                InitializeVehicles();
                return Page();
            }

            return RedirectToPage("/NotFound");
        }

        private void InitializeVehicles()
        {
            Boats = _context.Boats
                .AsNoTracking()
                .Where(vehicle => vehicle.Status == ListingStatusTypes.WaitingApproval.ToString() ||
                        vehicle.Status == ListingStatusTypes.Closed.ToString())
                .ToList();

            Buses = _context.Buses
                .AsNoTracking()
                .Where(vehicle => vehicle.Status == ListingStatusTypes.WaitingApproval.ToString() ||
                        vehicle.Status == ListingStatusTypes.Closed.ToString())
                .ToList();

            Cars = _context.Cars
                .AsNoTracking()
                .Where(vehicle => vehicle.Status == ListingStatusTypes.WaitingApproval.ToString() ||
                        vehicle.Status == ListingStatusTypes.Closed.ToString())
                .ToList();

            Motorcycles = _context.Motorcycles
                .AsNoTracking()
                .Where(vehicle => vehicle.Status == ListingStatusTypes.WaitingApproval.ToString() ||
                        vehicle.Status == ListingStatusTypes.Closed.ToString())
                .ToList();

            Trailers = _context.Trailers
                .AsNoTracking()
                .Where(vehicle => vehicle.Status == ListingStatusTypes.WaitingApproval.ToString() ||
                        vehicle.Status == ListingStatusTypes.Closed.ToString())
                .ToList();

            Trucks = _context.Trucks
                .AsNoTracking()
                .Where(vehicle => vehicle.Status == ListingStatusTypes.WaitingApproval.ToString() ||
                        vehicle.Status == ListingStatusTypes.Closed.ToString())
                .ToList();

            OverallCount = Boats.Count + Buses.Count + Cars.Count +
                        Motorcycles.Count + Trailers.Count + Trucks.Count;
        }

        public string GetImage(string vehicleId)
        {
            byte[] data = _context.VehicleImages
                .AsNoTracking()
                .First(i => i.VehicleId == vehicleId)
                .ImageData;

            string imgDataURL = "data:image;base64," + Convert.ToBase64String(data);
            return imgDataURL;
        }

        public bool HasAnyImages(string vehicleId)
        {
            return _context.VehicleImages
                .Where(img => img.VehicleId == vehicleId)
                .ToList().Count > 0;
        }
    }
}
