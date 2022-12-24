using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopAutoSpot.Data;
using TopAutoSpot.Models;
using TopAutoSpot.Models.Utilities;
using TopAutoSpot.Services.EmailService;
using TopAutoSpot.Views.Utilities;

namespace TopAutoSpot.Views.AdministratorViews.ApprovalViews
{
    [Authorize]
    public class ApproveAuctionModel : PageModel
    {
        private ApplicationDbContext _context;
        private IEmailService _emailService;

        public ApproveAuctionModel(ApplicationDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public string AuctionId { get; set; }

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

                    string currentUserId = UserServices.GetCurrentUser(_context, User.Identity.Name);

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
                        To = owner.Email,
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

        public bool AuctionApproved(string auctionId)
        {
            _context.Auctions.First(a => a.Id == auctionId).Status = ListingStatusTypes.Active.ToString();
            _context.SaveChanges();

            return true;
        }
    }
}
