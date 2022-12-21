using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopAutoSpot.Data;
using TopAutoSpot.Models.Utilities;
using TopAutoSpot.Views.Utilities;

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

        public async Task<IActionResult> OnGetAsync(string vehicleId, string reason)
        {
            if (User.IsInRole("Administrator"))
            {
                var refuseResult = await VehicleRefused(vehicleId);

                if (refuseResult)
                {
                    VehicleId = vehicleId;

                    var ownerId = await UserServices.GetVehicleOwner(_context, VehicleId);

                    var currentUserId = await UserServices.GetCurrentUser(_context, User.Identity.Name);

                    if (ownerId == "" || ownerId == null || currentUserId == null || currentUserId == "")
                    {
                        return RedirectToPage("/NotFound");

                    }

                    var sendResult = await NotificationServices.Send(_context,
                        currentUserId,
                        ownerId,
                        DefaultNotificationMessages.LISTING_REFUSED_TITLE,
                        reason);

                    if (sendResult == false)
                    {
                        return RedirectToPage("/Error");
                    }

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
