using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using TopAutoSpot.Data.Models;
using TopAutoSpot.Services.EmailService;
using TopAutoSpot.Views.Utilities;

namespace TopAutoSpot.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterConfirmationModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailService _emailService;

        public RegisterConfirmationModel(UserManager<User> userManager, IEmailService emailService)
        {
            _userManager = userManager;
            _emailService = emailService;
        }

        public string EmailConfirmationUrl { get; set; }

        public async Task<IActionResult> OnGetAsync(string id, string returnUrl = null)
        {
            if (id == null)
            {
                return RedirectToPage("/Index");
            }

            returnUrl = returnUrl ?? Url.Content("~/");

            User? user = await _userManager.FindByIdAsync(id);
            if (user == null || user.Email == null)
            {
                return NotFound($"Unable to load user with Id '{id}'.");
            }

            string userId = await _userManager.GetUserIdAsync(user);

            string code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            EmailConfirmationUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { area = "Identity", id = userId },
                protocol: Request.Scheme);

            _emailService.SendEmail(new EmailDto()
            {
                To = user.Email,
                Subject = DefaultNotificationMessages.REGISTER_CONFIRMATION_TITLE,
                Body = string.Format(DefaultNotificationMessages.REGISTER_CONFIRMATION_DESCRIPTION,
                    EmailConfirmationUrl)
            });

            return Page();
        }
    }
}
