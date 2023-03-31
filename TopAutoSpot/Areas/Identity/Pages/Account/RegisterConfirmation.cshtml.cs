using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopAutoSpot.Data.Models;
using TopAutoSpot.Services.Common;
using TopAutoSpot.Services.EmailServices;

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

        public string EmailConfirmationUrl { get; set; } = null!;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return RedirectToPage("/Index");
            }

            User? user = await _userManager.FindByIdAsync(id);
            if (user == null || user.Email == null)
            {
                return NotFound($"Unable to load user with Id '{id}'.");
            }

            string userId = await _userManager.GetUserIdAsync(user);

            EmailConfirmationUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { area = "Identity", id = userId },
                protocol: Request.Scheme)!;

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
