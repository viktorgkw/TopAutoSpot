using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TopAutoSpot.Views.AdministratorViews.Utilities
{
    [Authorize]
    public class RefuseVehicleReasonModel : PageModel
    {
        [BindProperty]
        public string Reason { get; set; } = null!;

        public string VehicleId { get; set; } = null!;

        public IActionResult OnGet(string vehicleId)
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

        public IActionResult OnPost()
        {
            return RedirectToPage("/AdministratorViews/ApprovalViews/RefuseVehicle",
                new { vehicleId = VehicleId, reason = Reason });
        }
    }
}
