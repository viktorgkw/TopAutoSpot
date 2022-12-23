using TopAutoSpot.Models.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;

namespace TopAutoSpot.Views.PremiumAccount
{
    [Authorize]
    public class IndexModel : PageModel
    {
        public async Task<IActionResult> OnGetAsync()
        {
            if (!User.IsInRole(RoleTypes.User.ToString()))
            {
                return RedirectToPage("/NotFound");
            }

            return Page();
        }
    }
}
