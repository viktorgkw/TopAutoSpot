using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopAutoSpot.Data;
using TopAutoSpot.Data.Models;
using TopAutoSpot.Data.Models.Enums;

namespace TopAutoSpot.Views.PremiumAccount
{
    [Authorize]
    public class PaymentResultModel : PageModel
    {
        private UserManager<User> _userManager;
        private ApplicationDbContext _context;
        public PaymentResultModel(UserManager<User> userManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public string Status { get; set; }

        public async Task<IActionResult> OnGetAsync(string status, string userId)
        {
            if (!User.IsInRole(RoleTypes.User.ToString()))
            {
                return RedirectToPage("/NotFound");
            }

            Status = status.ToLower();

            if (Status == "succeeded")
            {
                var foundUser = _context.Users.First(u => u.Id == userId);
                await _userManager.RemoveFromRoleAsync(foundUser, "User");
                await _userManager.AddToRoleAsync(foundUser, "Premium");
                foundUser.Role = "Premium";
                _context.SaveChanges();
            }

            return Page();
        }
    }
}
