namespace TopAutoSpot.Views.AdministratorViews.ApprovalViews
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
    /// A page model to handle approving an auction by an administrator.
    /// </summary>
    [Authorize]
    public class ApproveAuctionModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApproveAuctionModel"/> class.
        /// </summary>
        /// <param name="context">The application database context.</param>
        /// <param name="emailService">The email service used to send notifications.</param>
        public ApproveAuctionModel(ApplicationDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        /// <summary>
        /// Gets or sets the ID of the auction to approve.
        /// </summary>
        public string AuctionId { get; set; } = null!;

        /// <summary>
        /// Handles GET requests to the page.
        /// </summary>
        /// <param name="auctionId">The ID of the auction to approve.</param>
        /// <returns>The page result.</returns>
        public IActionResult OnGet(string auctionId)
        {
            if (User.IsInRole("Administrator"))
            {
                bool approveResult = AuctionApproved(auctionId);

                if (approveResult)
                {
                    AuctionId = auctionId;

                    string ownerId = UserServices.GetAuctionOwner(_context, AuctionId);
                    User owner = UserServices.GetUserById(_context, ownerId);

                    string currentUserId = UserServices.GetCurrentUser(_context, User.Identity!.Name!);

                    bool sendResult = NotificationServices.Send(_context,
                        currentUserId,
                        ownerId,
                        DefaultNotificationMessages.AUCTION_APPROVED_TITLE,
                        DefaultNotificationMessages.AUCTION_APPROVED_DESCRIPTION);

                    if (!sendResult)
                    {
                        return RedirectToPage("/UnknownError");
                    }

                    _emailService.SendEmail(new EmailDto()
                    {
                        To = owner.Email!,
                        Subject = DefaultNotificationMessages.AUCTION_APPROVED_TITLE,
                        Body = DefaultNotificationMessages.AUCTION_APPROVED_DESCRIPTION
                    });

                    return Page();
                }
                else
                {
                    return RedirectToPage("/UnknownError");
                }
            }

            return RedirectToPage("/NotFound");
        }

        /// <summary>
        /// Approves the specified auction.
        /// </summary>
        /// <param name="auctionId">The ID of the auction to approve.</param>
        /// <returns><c>true</c> if the auction was approved successfully; otherwise, <c>false</c>.</returns>
        public bool AuctionApproved(string auctionId)
        {
            _context.Auctions.First(a => a.Id == auctionId).Status = ListingStatusTypes.Active.ToString();
            _context.SaveChanges();

            return true;
        }
    }
}
