using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;

namespace TopAutoSpot.Views.AdministratorViews.Utilities
{
    [Authorize]
    public class RefuseVehicleReasonModel : PageModel
    {
        [BindProperty]
        public string Reason { get; set; }
        public string VehicleId { get; set; }

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
            return RedirectToPage("/AdministratorViews/ApprovalViews/RefuseVehicle", new { vehicleId = VehicleId, reason = Reason });
        }
    }
}
