using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using TopAutoSpot.Data;
using TopAutoSpot.Data.Entities.Utilities;

namespace TopAutoSpot.Views.AdministratorViews.Utilities
{
    [Authorize]
    public class ApproveVehicleModel : PageModel
    {
        private ApplicationDbContext _context;

        public ApproveVehicleModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string VehicleId { get; set; }

        public async Task<IActionResult> OnGetAsync(string vehicleId)
        {
            if (User.IsInRole("Administrator"))
            {
                var result = await VehicleApproved(vehicleId);

                if (result)
                {
                    VehicleId = vehicleId;

                    var ownerId = await GetVehicleOwner(VehicleId);

                    var currentUserId = await GetCurrentUser();

                    await NotificationServices.Send(_context,
                        currentUserId,
                        ownerId,
                        DefaultNotificationMessages.LISTING_APPROVED_TITLE,
                        DefaultNotificationMessages.LISTING_APPROVED_DESCRIPTION);

                    return Page();
                }
                else
                {
                    return RedirectToPage("/UnknownError");
                }
            }

            return RedirectToPage("/NotFound");
        }

        public async Task<bool> VehicleApproved(string vehicleId)
        {
            if (_context.Cars.FirstOrDefault(c => c.Id == vehicleId) != null)
            {
                _context.Cars
                    .FirstOrDefault(c => c.Id == vehicleId).Status = StatusTypes.Active.ToString();
                await _context.SaveChangesAsync();

                return true;
            }
            else if (_context.Motorcycles.FirstOrDefault(m => m.Id == vehicleId) != null)
            {
                _context.Motorcycles
                    .FirstOrDefault(m => m.Id == vehicleId).Status = StatusTypes.Active.ToString();
                await _context.SaveChangesAsync();

                return true;
            }
            else if (_context.Trucks.FirstOrDefault(t => t.Id == vehicleId) != null)
            {
                _context.Trucks
                    .FirstOrDefault(t => t.Id == vehicleId).Status = StatusTypes.Active.ToString();
                await _context.SaveChangesAsync();

                return true;
            }
            else if (_context.Trailers.FirstOrDefault(t => t.Id == vehicleId) != null)
            {
                _context.Trailers
                    .FirstOrDefault(t => t.Id == vehicleId).Status = StatusTypes.Active.ToString();
                await _context.SaveChangesAsync();

                return true;
            }
            else if (_context.Buses.FirstOrDefault(b => b.Id == vehicleId) != null)
            {
                _context.Buses
                    .FirstOrDefault(b => b.Id == vehicleId).Status = StatusTypes.Active.ToString();
                await _context.SaveChangesAsync();

                return true;
            }
            else if (_context.Boats.FirstOrDefault(b => b.Id == vehicleId) != null)
            {
                _context.Boats.FirstOrDefault(b => b.Id == vehicleId).Status = StatusTypes.Active.ToString();
                await _context.SaveChangesAsync();

                return true;
            }
            else
            {
                return false;
            }
        }

        private async Task<string> GetCurrentUser()
        {
            return _context.Users.First(u => u.UserName == User.Identity.Name).Id;
        }

        private async Task<string> GetVehicleOwner(string vehicleId)
        {
            var Boats = await _context.Boats
                .Where(boat => boat.Id == vehicleId)
                .ToListAsync();

            if (Boats.Count > 0)
            {
                return Boats.First().CreatedBy;
            }

            var Buses = await _context.Buses
                .Where(bus => bus.Id == vehicleId)
                .ToListAsync();

            if (Buses.Count > 0)
            {
                return Buses.First().CreatedBy;
            }

            var Cars = await _context.Cars
                .Where(car => car.Id == vehicleId)
                .ToListAsync();

            if (Cars.Count > 0)
            {
                return Cars.First().CreatedBy;
            }

            var Motorcycles = await _context.Motorcycles
                .Where(motorcycle => motorcycle.Id == vehicleId)
                .ToListAsync();

            if (Motorcycles.Count > 0)
            {
                return Motorcycles.First().CreatedBy;
            }

            var Trailers = await _context.Trailers
                .Where(trailer => trailer.Id == vehicleId)
                .ToListAsync();

            if (Trailers.Count > 0)
            {
                return Trailers.First().CreatedBy;
            }

            var Trucks = await _context.Trucks
                .Where(truck => truck.Id == vehicleId)
                .ToListAsync();

            if (Trucks.Count > 0)
            {
                return Trucks.First().CreatedBy;
            }

            return "";
        }
    }
}
