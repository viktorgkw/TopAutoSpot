using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopAutoSpot.Data;
using TopAutoSpot.Data.Models;
using TopAutoSpot.Services.Common;
using TopAutoSpot.Services.EmailServices;
using TopAutoSpot.Services.Utilities;

namespace TopAutoSpot.Areas.Identity.Pages.Account.Manage
{
    public class DeletePersonalDataConfirmationModel : PageModel
    {
        private VehicleRemover _vr;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ApplicationDbContext _context;
        private IEmailService _emailService;

        public DeletePersonalDataConfirmationModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ApplicationDbContext db,
            IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = db;
            _emailService = emailService;
            _vr = new VehicleRemover(_context);
        }

        public async Task<IActionResult> OnGetAsync(string userId)
        {
            User? foundUser = await _userManager.FindByIdAsync(userId);

            if (foundUser is null)
            {
                return RedirectToPage("/NotFound");
            }

            IdentityResult result = await _userManager.DeleteAsync(foundUser);

            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Unexpected error occurred deleting user.");
            }

            await _vr.RemoveAllUserVehicles(userId);

            await _signInManager.SignOutAsync();

            _emailService.SendEmail(new EmailDto()
            {
                To = foundUser.Email!,
                Subject = DefaultNotificationMessages.ACCOUNT_DELETED_SUCCESSFULLY_TITLE,
                Body = DefaultNotificationMessages.ACCOUNT_DELETED_SUCCESSFULLY_DESCRIPTION
            });

            return RedirectToPage("/Index");
        }
    }
}
