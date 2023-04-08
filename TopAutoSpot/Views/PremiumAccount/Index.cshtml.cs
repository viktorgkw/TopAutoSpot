namespace TopAutoSpot.Views.PremiumAccount
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    using TopAutoSpot.Data.Models.Enums;

    /// <summary>
    /// A page model class decorated with [Authorize] attribute that allows only authenticated users to access it.
    /// </summary>
    [Authorize]
    public class IndexModel : PageModel
    {
        /// <summary>
        /// This method handles GET requests and verifies that the authenticated user has the "User" role.
        /// If the user does not have the "User" role, they are redirected to the "NotFound" page.
        /// </summary>
        /// <returns>Returns a page result for the current page.</returns>
        public IActionResult OnGet()
        {
            if (!User.IsInRole(RoleTypes.User.ToString()))
            {
                return RedirectToPage("/NotFound");
            }

            return Page();
        }
    }
}
