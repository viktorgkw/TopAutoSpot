using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using TopAutoSpot.Data.Models;
using TopAutoSpot.Services.Common;
using TopAutoSpot.Services.EmailServices;

namespace TopAutoSpot.Areas.Identity.Pages.Account.Manage
{
    public class DeletePersonalDataModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailService _emailService;

        public DeletePersonalDataModel(
            UserManager<User> userManager,
            IEmailService emailService)
        {
            _userManager = userManager;
            _emailService = emailService;
        }

        [BindProperty]
        public InputModel Input { get; set; } = null!;

        public class InputModel
        {
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; } = null!;
        }

        public bool RequirePassword { get; set; }

        public async Task<IActionResult> OnGet()
        {
            User? user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            RequirePassword = await _userManager.HasPasswordAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            User? user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            RequirePassword = await _userManager.HasPasswordAsync(user);
            if (RequirePassword)
            {
                if (!await _userManager.CheckPasswordAsync(user, Input.Password))
                {
                    ModelState.AddModelError(string.Empty, "Incorrect password.");
                    return Page();
                }
            }

            string userId = await _userManager.GetUserIdAsync(user);

            string? callbackUrl = Url.Page(
                "/Account/Manage/DeletePersonalDataConfirmation",
                pageHandler: null,
                values: new { area = "Identity", userId },
                protocol: Request.Scheme);

            _emailService.SendEmail(new EmailDto()
            {
                To = user.Email!,
                Subject = DefaultNotificationMessages.ACCOUNT_DELETE_CONFIRMATION_TITLE,
                Body = string.Format(DefaultNotificationMessages.ACCOUNT_DELETE_CONFIRMATION_DESCRIPTION, callbackUrl)
            });

            return RedirectToPage("/Account/CheckYourEmail", new { area = "Identity" });
        }
    }
}
