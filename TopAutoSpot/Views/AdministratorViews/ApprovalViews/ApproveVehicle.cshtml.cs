using TopAutoSpot.Data;
using TopAutoSpot.Views.Utilities;
using TopAutoSpot.Models.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;

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
                var approveResult = await VehicleApproved(vehicleId);

                if (approveResult)
                {
                    VehicleId = vehicleId;

                    var ownerId = await UserServices.GetVehicleOwner(_context, VehicleId);

                    var currentUserId = await UserServices.GetCurrentUser(_context, User.Identity.Name);

                    var sendResult = await NotificationServices.Send(_context,
                        currentUserId,
                        ownerId,
                        DefaultNotificationMessages.LISTING_APPROVED_TITLE,
                        DefaultNotificationMessages.LISTING_APPROVED_DESCRIPTION);

                    if (!sendResult)
                    {
                        return RedirectToPage("/UnknownError");
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
    }
}
