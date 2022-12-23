using TopAutoSpot.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace TopAutoSpot.Views.PremiumAccount
{
    [Authorize]
    public class PaymentResultModel : PageModel
    {
        private UserManager<User> _userManager;

        public string Status { get; set; }

        public async Task<IActionResult> OnGetAsync(string status, string userId)
        {
            Status = status.ToLower();

            if (Status == "succeeded")
            {
                var foundUser = await _userManager.FindByIdAsync(userId);
                await _userManager.AddToRoleAsync(foundUser, "Premium");
            }

            return Page();
        }
    }
}
