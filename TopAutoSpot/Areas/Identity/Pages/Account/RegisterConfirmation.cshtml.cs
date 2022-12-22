using System.Text;
using TopAutoSpot.Models;
using TopAutoSpot.Views.Utilities;
using TopAutoSpot.Services.EmailService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.AspNetCore.Authorization;

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

            var user = await _userManager.FindByIdAsync(id);
            if (user == null || user.Email == null)
            {
                return NotFound($"Unable to load user with Id '{id}'.");
            }

            var userId = await _userManager.GetUserIdAsync(user);

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
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
