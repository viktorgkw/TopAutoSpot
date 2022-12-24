using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopAutoSpot.Data;
using TopAutoSpot.Models;
using TopAutoSpot.Services.EmailService;
using TopAutoSpot.Views.Utilities;

namespace TopAutoSpot.Areas.Identity.Pages.Account.Manage
{
    public class DeletePersonalDataConfirmationModel : PageModel
    {
        private VehicleRemover _vr;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<DeletePersonalDataModel> _logger;
        private readonly ApplicationDbContext _context;
        private IEmailService _emailService;

        public DeletePersonalDataConfirmationModel(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ILogger<DeletePersonalDataModel> logger,
            ApplicationDbContext db,
            IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _context = db;
            _emailService = emailService;
            _vr = new VehicleRemover(_context);
        }

        public async Task<IActionResult> OnGetAsync(string userId)
        {
            User? foundUser = await _userManager.FindByIdAsync(userId);
            IdentityResult result = await _userManager.DeleteAsync(foundUser);

            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Unexpected error occurred deleting user.");
            }

            _vr.RemoveAllUserVehicles(userId);

            await _signInManager.SignOutAsync();

            _logger.LogInformation($"User with ID '{userId}' deleted themselves.");

            _emailService.SendEmail(new EmailDto()
            {
                To = foundUser.Email,
                Subject = DefaultNotificationMessages.ACCOUNT_DELETED_SUCCESSFULLY_TITLE,
                Body = DefaultNotificationMessages.ACCOUNT_DELETED_SUCCESSFULLY_DESCRIPTION
            });

            return RedirectToPage("/Index");
        }
    }
}
