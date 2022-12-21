using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TopAutoSpot.Data;
using TopAutoSpot.Data.Entities.Utilities;

namespace TopAutoSpot.Views.AdministratorViews.Utilities
{
    [Authorize]
    public class RefuseVehicleReasonModel : PageModel
    {
        private ApplicationDbContext _context;

        public RefuseVehicleReasonModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string VehicleId { get; set; }

        [BindProperty]
        public string Reason { get; set; }

        public async Task<IActionResult> OnGetAsync(string vehicleId)
        {
            if (User.IsInRole("Administrator"))
            {
                if (vehicleId == null)
                {
                    return RedirectToPage("/NotFound");
                }

                VehicleId = vehicleId;

                return Page();
            }

            return RedirectToPage("/NotFound");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var ownerId = await GetVehicleOwner(VehicleId);

            var currentUserId = await GetCurrentUser();

            if (ownerId == "" || ownerId == null || currentUserId == null || currentUserId == "")
            {
                return RedirectToPage("/NotFound");

            }

            var result = await NotificationServices.Send(_context,
                currentUserId,
                ownerId,
                DefaultNotificationMessages.LISTING_REFUSED_TITLE,
                Reason);

            if (result == false)
            {
                return RedirectToPage("/Error");
            }

            return RedirectToPage("/AdministratorViews/Utilities/RefuseVehicle", new { vehicleId = VehicleId });
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
