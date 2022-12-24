using TopAutoSpot.Data;
using TopAutoSpot.Models;
using TopAutoSpot.Views.Utilities;
using TopAutoSpot.Services.EmailService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace TopAutoSpot.Views.AdministratorViews.UsersCRUD
{
    [Authorize]
    public class CloseModel : PageModel
    {
        private ApplicationDbContext _context;
        private IEmailService _emailService;
        private VehicleRemover _vehicleRemover;

        public CloseModel(ApplicationDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
            _vehicleRemover = new VehicleRemover(_context);
        }

        [BindProperty]
        public User UserToClose { get; set; }

        public async Task<IActionResult> OnGetAsync(string userId)
        {
            if (User.IsInRole("Administrator"))
            {
                UserToClose = await _context.Users.FirstAsync(u => u.Id == userId);

                _context.Users.Remove(UserToClose);
                await _context.SaveChangesAsync();

                _vehicleRemover.RemoveAllUserVehicles(UserToClose.Id);
                _vehicleRemover.RemoveAllUserAuctions(UserToClose.Id);

                _emailService.SendEmail(new EmailDto()
                {
                    To = UserToClose.Email,
                    Subject = DefaultNotificationMessages.ACCOUNT_CLOSED_TITLE,
                    Body = DefaultNotificationMessages.ACCOUNT_CLOSED_DESCRIPTION
                });

                return Page();
            }

            return RedirectToPage("/NotFound");
        }
    }
}
