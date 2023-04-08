namespace TopAutoSpot.Views.AdministratorViews.AuctionsCD
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
    /// Represents the page model for deleting an auction with authorization.
    /// </summary>
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeleteModel"/> class with the specified dependencies.
        /// </summary>
        /// <param name="context">The application database context.</param>
        /// <param name="emailService">The email service.</param>
        public DeleteModel(ApplicationDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        /// <summary>
        /// Gets or sets the auction to be deleted.
        /// </summary>
        [BindProperty]
        public Auction AuctionToDelete { get; set; } = null!;

        /// <summary>
        /// Handles GET requests for deleting an auction.
        /// </summary>
        /// <param name="id">The ID of the auction to be deleted.</param>
        /// <returns>The page result.</returns>
        public IActionResult OnGet(string id)
        {
            if (User.IsInRole("Administrator"))
            {
                AuctionToDelete = _context.Auctions.First(u => u.Id == id);
                User owner = _context.Users.First(u => u.Id == AuctionToDelete.AuctioneerId);
                string currentUser = UserServices.GetCurrentUser(_context, User.Identity!.Name!);

                _context.Auctions.Remove(AuctionToDelete);
                _context.SaveChanges();

                NotificationServices.Send(_context,
                    currentUser,
                    owner.Id,
                    DefaultNotificationMessages.AUCTION_DELETED_TITLE,
                    DefaultNotificationMessages.AUCTION_DELETED_DESCRIPTION);

                _emailService.SendEmail(new EmailDto()
                {
                    To = owner.Email!,
                    Subject = DefaultNotificationMessages.AUCTION_DELETED_TITLE,
                    Body = DefaultNotificationMessages.AUCTION_DELETED_DESCRIPTION
                });

                return Page();
            }

            return RedirectToPage("/NotFound");
        }
    }
}
