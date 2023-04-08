namespace TopAutoSpot.Views.AdministratorViews.UsersCRUD
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    using TopAutoSpot.Data;
    using TopAutoSpot.Data.Models;
    using TopAutoSpot.Services.Common;
    using TopAutoSpot.Services.EmailServices;
    using TopAutoSpot.Services.Utilities;

    /// <summary>
    /// Page model for closing a user account.
    /// Requires authorization, only accessible to administrators.
    /// </summary>
    [Authorize]
    public class CloseModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;
        private readonly VehicleRemover _vehicleRemover;

        /// <summary>
        /// Constructor for CloseModel.
        /// </summary>
        /// <param name="context">The ApplicationDbContext.</param>
        /// <param name="emailService">The IEmailService.</param>
        public CloseModel(ApplicationDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
            _vehicleRemover = new VehicleRemover(_context);
        }

        [BindProperty]
        public User UserToClose { get; set; } = null!;

        /// <summary>
        /// GET request method for closing a user account.
        /// </summary>
        /// <param name="userId">The ID of the user account to be closed.</param>
        /// <returns>The page if the user is an administrator, else NotFound.</returns>
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
