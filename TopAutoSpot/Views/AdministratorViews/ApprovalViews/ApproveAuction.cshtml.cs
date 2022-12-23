using TopAutoSpot.Data;
using TopAutoSpot.Models;
using TopAutoSpot.Models.Utilities;
using TopAutoSpot.Views.Utilities;
using TopAutoSpot.Services.EmailService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;

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

        public async Task<IActionResult> OnGetAsync(string auctionId)
        {
            if (User.IsInRole("Administrator"))
            {
                var approveResult = await AuctionApproved(auctionId);

                if (approveResult)
                {
                    AuctionId = auctionId;

                    var ownerId = UserServices.GetAuctionOwner(_context, AuctionId);
                    var owner = await UserServices.GetUserById(_context, ownerId);

                    var currentUserId = await UserServices.GetCurrentUser(_context, User.Identity.Name);

                    var sendResult = await NotificationServices.Send(_context,
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

        public async Task<bool> AuctionApproved(string auctionId)
        {
            _context.Auctions
                    .First(a => a.Id == auctionId).Status = ListingStatusTypes.Active.ToString();
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
