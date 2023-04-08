namespace TopAutoSpot.Views.AdministratorViews.AuctionsCD
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    using TopAutoSpot.Data;
    using TopAutoSpot.Data.Models;
    using TopAutoSpot.Data.Models.Enums;
    using TopAutoSpot.Services.Common;
    using TopAutoSpot.Services.EmailServices;
    using TopAutoSpot.Services.Utilities;

    /// <summary>
    /// The CloseModel class represents a PageModel used to close an auction.
    /// </summary>
    [Authorize]
    public class CloseModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;

        public CloseModel(ApplicationDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        /// <summary>
        /// Gets or sets the auction to close.
        /// </summary>
        [BindProperty]
        public Auction AuctionToClose { get; set; } = null!;

        /// <summary>
        /// Handles the GET request for the Close page.
        /// </summary>
        /// <param name="id">The ID of the auction to close.</param>
        /// <returns>The result of the action.</returns>
        public IActionResult OnGet(string id)
        {
            if (User.IsInRole("Administrator"))
            {
                _context.Auctions.First(u => u.Id == id).Status = AuctionStatusTypes.Closed.ToString();
                _context.SaveChanges();

                User owner = UserServices.GetUserById(_context, AuctionToClose.AuctioneerId!);
                string currentUser = UserServices.GetCurrentUser(_context, User.Identity!.Name!);

                NotificationServices.Send(_context,
                    currentUser,
                    owner.Id,
                    DefaultNotificationMessages.AUCTION_CLOSED_TITLE,
                    DefaultNotificationMessages.AUCTION_CLOSED_DESCRIPTION);

                _emailService.SendEmail(new EmailDto()
                {
                    To = owner.Email!,
                    Subject = DefaultNotificationMessages.AUCTION_CLOSED_TITLE,
                    Body = DefaultNotificationMessages.AUCTION_CLOSED_DESCRIPTION
                });

                return Page();
            }

            return RedirectToPage("/NotFound");
        }
    }
}
