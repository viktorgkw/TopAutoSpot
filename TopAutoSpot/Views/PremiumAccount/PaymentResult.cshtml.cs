namespace TopAutoSpot.Views.PremiumAccount
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    using TopAutoSpot.Data;
    using TopAutoSpot.Data.Models;
    using TopAutoSpot.Data.Models.Enums;

    /// <summary>
    /// Page model for the payment result page that is only accessible to authorized users.
    /// </summary>
    [Authorize]
    public class PaymentResultModel : PageModel
    {
        private readonly UserManager<User> _userManager;

        private readonly ApplicationDbContext _context;

        public PaymentResultModel(UserManager<User> userManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        /// <summary>
        /// The status of the payment.
        /// </summary>
        public string Status { get; set; } = null!;

        /// <summary>
        /// Handles the HTTP GET request to the payment result page.
        /// </summary>
        /// <param name="status">The status of the payment.</param>
        /// <param name="userId">The ID of the user making the payment.</param>
        /// <returns>The payment result page.</returns>
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
