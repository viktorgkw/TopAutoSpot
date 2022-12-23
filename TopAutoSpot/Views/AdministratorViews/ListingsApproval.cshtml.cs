using TopAutoSpot.Data;
using TopAutoSpot.Models;
using TopAutoSpot.Models.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

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

        public async Task<IActionResult> OnGetAsync()
        {
            if (User.IsInRole("Administrator"))
            {
                await InitializeVehicles();
                return Page();
            }

            return RedirectToPage("/NotFound");
        }

        private async Task InitializeVehicles()
        {
            Boats = await _context.Boats
                    .Where(vehicle => vehicle.Status == ListingStatusTypes.WaitingApproval.ToString() ||
                            vehicle.Status == ListingStatusTypes.Closed.ToString())
                    .ToListAsync();

            Buses = await _context.Buses
                    .Where(vehicle => vehicle.Status == ListingStatusTypes.WaitingApproval.ToString() ||
                            vehicle.Status == ListingStatusTypes.Closed.ToString())
                    .ToListAsync();

            Cars = await _context.Cars
                    .Where(vehicle => vehicle.Status == ListingStatusTypes.WaitingApproval.ToString() ||
                            vehicle.Status == ListingStatusTypes.Closed.ToString())
                    .ToListAsync();

            Motorcycles = await _context.Motorcycles
                    .Where(vehicle => vehicle.Status == ListingStatusTypes.WaitingApproval.ToString() ||
                            vehicle.Status == ListingStatusTypes.Closed.ToString())
                    .ToListAsync();

            Trailers = await _context.Trailers
                    .Where(vehicle => vehicle.Status == ListingStatusTypes.WaitingApproval.ToString() ||
                            vehicle.Status == ListingStatusTypes.Closed.ToString())
                    .ToListAsync();

            Trucks = await _context.Trucks
                    .Where(vehicle => vehicle.Status == ListingStatusTypes.WaitingApproval.ToString() ||
                            vehicle.Status == ListingStatusTypes.Closed.ToString())
                    .ToListAsync();

            OverallCount = Boats.Count + Buses.Count + Cars.Count +
                        Motorcycles.Count + Trailers.Count + Trucks.Count;
        }

        public string GetImage(string vehicleId)
        {
            var data = _context.VehicleImages.First(i => i.VehicleId == vehicleId).ImageData;
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