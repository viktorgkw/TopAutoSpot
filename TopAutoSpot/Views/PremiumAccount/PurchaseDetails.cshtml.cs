namespace TopAutoSpot.Views.PremiumAccount
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    using TopAutoSpot.Data.Models;
    using TopAutoSpot.Data.Models.Enums;
    using TopAutoSpot.Services.PaymentServices;

    /// <summary>
    /// Page model for PurchaseDetails page that handles payment processing and redirects to payment result page based on payment outcome.
    /// </summary>
    [Authorize]
    public class PurchaseDetailsModel : PageModel
    {
        private readonly IPaymentService _paymentService;

        private readonly UserManager<User> _userManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="PurchaseDetailsModel"/> class.
        /// </summary>
        /// <param name="paymentService">The payment service.</param>
        /// <param name="userManager">The user manager.</param>
        public PurchaseDetailsModel(IPaymentService paymentService, UserManager<User> userManager)
        {
            _paymentService = paymentService;
            _userManager = userManager;
        }

        /// <summary>
        /// Gets or sets the Stripe payment.
        /// </summary>
        [BindProperty]
        public StripePayment StripePayment { get; set; } = null!;

        /// <summary>
        /// HTTP GET method for PurchaseDetails page. Ensures user is logged in with 'User' role, otherwise redirects to NotFound page.
        /// </summary>
        /// <returns>The page.</returns>
        public IActionResult OnGet()
        {
            if (!User.IsInRole(RoleTypes.User.ToString()))
            {
                return RedirectToPage("/NotFound");
            }

            return Page();
        }

        /// <summary>
        /// HTTP POST method for PurchaseDetails page. Handles payment processing and redirects to payment result page based on payment outcome.
        /// </summary>
        /// <returns>The payment result page.</returns>
        public async Task<IActionResult> OnPostAsync()
        {
            var foundUser = await _userManager.FindByNameAsync(User.Identity!.Name!);
            StripePayment.Email = foundUser!.Email!;

            string[] years = new string[] { "2023", "2024", "2025", "2026", "2027", "2028" };

            if (!years.Contains(StripePayment.ExpYear))
            {
                return RedirectToPage("/PremiumAccount/PaymentResult", new { status = "incorrect" });
            }

            string[] months = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" };

            if (!months.Contains(StripePayment.ExpMonth))
            {
                return RedirectToPage("/PremiumAccount/PaymentResult", new { status = "incorrect" });
            }

            try
            {
                string result = _paymentService.MakePayment(StripePayment);

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
