namespace TopAutoSpot.Views.AdministratorViews.ApprovalViews
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    /// <summary>
    /// PageModel class that handles the administrator's input of a reason to refuse a vehicle.
    /// </summary>
    [Authorize]
    public class RefuseVehicleReasonModel : PageModel
    {
        [BindProperty]
        public string Reason { get; set; } = null!;

        public string VehicleId { get; set; } = null!;

        /// <summary>
        /// Handles GET requests for the page, checks the user's authorization level, and sets the VehicleId property.
        /// </summary>
        /// <param name="vehicleId">The id of the vehicle to refuse.</param>
        /// <returns>If authorized, returns the page. Otherwise, returns a NotFound page.</returns>
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

        /// <summary>
        /// Handles POST requests for the page and redirects to the RefuseVehicle page.
        /// </summary>
        /// <returns>A redirect to the RefuseVehicle page with the vehicleId and reason parameters.</returns>
        public IActionResult OnPost()
        {
            return RedirectToPage("/AdministratorViews/ApprovalViews/RefuseVehicle",
                new { vehicleId = VehicleId, reason = Reason });
        }
    }
}
