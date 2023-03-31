using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopAutoSpot.Data;
using TopAutoSpot.Data.Models;
using TopAutoSpot.Services.Common;
using TopAutoSpot.Services.EmailServices;
using TopAutoSpot.Views.Utilities;

namespace TopAutoSpot.Views.AdministratorViews.UsersCRUD
{
    [Authorize]
    public class CloseModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;
        private readonly VehicleRemover _vehicleRemover;

        public CloseModel(ApplicationDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
            _vehicleRemover = new VehicleRemover(_context);
        }

        [BindProperty]
        public User UserToClose { get; set; } = null!;

        public IActionResult OnGet(string userId)
        {
            if (User.IsInRole("Administrator"))
            {
                UserToClose = _context.Users.First(u => u.Id == userId);

                _context.Users.Remove(UserToClose);
                _context.SaveChanges();

                _vehicleRemover.RemoveAllUserVehicles(UserToClose.Id);
                _vehicleRemover.RemoveAllUserAuctions(UserToClose.Id);

                _emailService.SendEmail(new EmailDto()
                {
                    To = UserToClose.Email!,
                    Subject = DefaultNotificationMessages.ACCOUNT_CLOSED_TITLE,
                    Body = DefaultNotificationMessages.ACCOUNT_CLOSED_DESCRIPTION
                });

                return Page();
            }

            return RedirectToPage("/NotFound");
        }
    }
}
