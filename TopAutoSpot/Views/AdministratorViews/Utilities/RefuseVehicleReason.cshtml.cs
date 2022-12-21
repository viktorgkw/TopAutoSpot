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
            return RedirectToPage("/AdministratorViews/Utilities/RefuseVehicle", new { vehicleId = VehicleId, reason = Reason });
        }
    }
}
