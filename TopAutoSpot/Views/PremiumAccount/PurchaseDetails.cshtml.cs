using TopAutoSpot.Models;
using TopAutoSpot.Models.Utilities;
using TopAutoSpot.Services.PaymentServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace TopAutoSpot.Views.PremiumAccount
{
    [Authorize]
    public class PurchaseDetailsModel : PageModel
    {
        private IPaymentService _paymentService;
        private UserManager<User> _userManager;
        public PurchaseDetailsModel(IPaymentService paymentService, UserManager<User> userManager)
        {
            _paymentService = paymentService;
            _userManager = userManager;
        }

        [BindProperty]
        public StripePayment StripePayment { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (!User.IsInRole(RoleTypes.User.ToString()))
            {
                return RedirectToPage("/NotFound");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var foundUser = await _userManager.FindByNameAsync(User.Identity.Name);
            StripePayment.Email = foundUser.Email;

            var years = new string[] { "2023", "2024", "2025", "2026", "2027", "2028" };

            if (!years.Contains(StripePayment.ExpYear))
            {
                return RedirectToPage("/PremiumAccount/PaymentResult", new { status = "incorrect" });
            }

            var months = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" };

            if (!months.Contains(StripePayment.ExpMonth))
            {
                return RedirectToPage("/PremiumAccount/PaymentResult", new { status = "incorrect" });
            }

            try
            {
                var result = await _paymentService.MakePayment(StripePayment);

                if (result == "succeeded" || result == "pending" || result == "failed")
                {
                    return RedirectToPage("/PremiumAccount/PaymentResult",
                        new { status = result, userId = foundUser.Id });
                }
                else
                {
                    return RedirectToPage("/UnknownError");
                }
            }
            catch
            {
                return RedirectToPage("/PremiumAccount/PaymentResult", new { status = "incorrect" });
            }
        }
    }
}
