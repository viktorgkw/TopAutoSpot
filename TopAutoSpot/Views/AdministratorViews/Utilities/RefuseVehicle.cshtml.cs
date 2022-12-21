using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopAutoSpot.Data;
using TopAutoSpot.Data.Entities.Utilities;

namespace TopAutoSpot.Views.AdministratorViews.Utilities
{
    public class RefuseVehicleModel : PageModel
    {
        private ApplicationDbContext _context;

        public RefuseVehicleModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string VehicleId { get; set; }

        public async Task<IActionResult> OnGetAsync(string vehicleId)
        {
            if (User.IsInRole("Administrator"))
            {
                var result = await VehicleRefused(vehicleId);

                if (result)
                {
                    VehicleId = vehicleId;
                    return Page();
                }
                else
                {
                    return RedirectToPage("/UnknownError");
                }
            }

            return RedirectToPage("/NotFound");
        }

        public async Task<bool> VehicleRefused(string vehicleId)
        {
            if (_context.Cars.FirstOrDefault(c => c.Id == vehicleId) != null)
            {
                _context.Cars
                    .FirstOrDefault(c => c.Id == vehicleId).Status = StatusTypes.Closed.ToString();
                await _context.SaveChangesAsync();

                return true;
            }
            else if (_context.Motorcycles.FirstOrDefault(m => m.Id == vehicleId) != null)
            {
                _context.Motorcycles
                    .FirstOrDefault(m => m.Id == vehicleId).Status = StatusTypes.Closed.ToString();
                await _context.SaveChangesAsync();

                return true;
            }
            else if (_context.Trucks.FirstOrDefault(t => t.Id == vehicleId) != null)
            {
                _context.Trucks
                    .FirstOrDefault(t => t.Id == vehicleId).Status = StatusTypes.Closed.ToString();
                await _context.SaveChangesAsync();

                return true;
            }
            else if (_context.Trailers.FirstOrDefault(t => t.Id == vehicleId) != null)
            {
                _context.Trailers
                    .FirstOrDefault(t => t.Id == vehicleId).Status = StatusTypes.Closed.ToString();
                await _context.SaveChangesAsync();

                return true;
            }
            else if (_context.Buses.FirstOrDefault(b => b.Id == vehicleId) != null)
            {
                _context.Buses
                    .FirstOrDefault(b => b.Id == vehicleId).Status = StatusTypes.Closed.ToString();
                await _context.SaveChangesAsync();

                return true;
            }
            else if (_context.Boats.FirstOrDefault(b => b.Id == vehicleId) != null)
            {
                _context.Boats.FirstOrDefault(b => b.Id == vehicleId).Status = StatusTypes.Closed.ToString();
                await _context.SaveChangesAsync();

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
