using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TopAutoSpot.Data;
using TopAutoSpot.Data.Entities;
using TopAutoSpot.Data.Entities.Utilities;

namespace TopAutoSpot.Views.AdministratorViews
{
    [Authorize]
    public class ApprovalModel : PageModel
    {
        private ApplicationDbContext _context;

        public ApprovalModel(ApplicationDbContext context)
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
            var foundUser = await _context.Users.FirstAsync(u => u.UserName == User.Identity.Name);

            if (foundUser == null)
            {
                return RedirectToPage("/Index");
            }
            else if (foundUser.Role != RoleTypes.Administrator.ToString())
            {
                return RedirectToPage("/NotFound");
            }
            else
            {
                await InitializeVehicles();
                return Page();
            }
        }

        private async Task InitializeVehicles()
        {
            Boats = await _context.Boats
                        .Where(vehicle => vehicle.Status == StatusTypes.WaitingApproval.ToString())
                        .ToListAsync();

            Buses = await _context.Buses
                    .Where(vehicle => vehicle.Status == StatusTypes.WaitingApproval.ToString())
                    .ToListAsync();

            Cars = await _context.Cars
                    .Where(vehicle => vehicle.Status == StatusTypes.WaitingApproval.ToString())
                    .ToListAsync();

            Motorcycles = await _context.Motorcycles
                    .Where(vehicle => vehicle.Status == StatusTypes.WaitingApproval.ToString())
                    .ToListAsync();

            Trailers = await _context.Trailers
                    .Where(vehicle => vehicle.Status == StatusTypes.WaitingApproval.ToString())
                    .ToListAsync();

            Trucks = await _context.Trucks
                    .Where(vehicle => vehicle.Status == StatusTypes.WaitingApproval.ToString())
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
