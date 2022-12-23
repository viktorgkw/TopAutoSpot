using TopAutoSpot.Data;
using TopAutoSpot.Models;
using TopAutoSpot.Views.Utilities;
using TopAutoSpot.Services.EmailService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using TopAutoSpot.Models.Utilities;

namespace TopAutoSpot.Views.AdministratorViews.ApprovalViews
{
    [Authorize]
    public class RefuseAuctionModel : PageModel
    {
        private ApplicationDbContext _context;
        private IEmailService _emailService;

        public RefuseAuctionModel(ApplicationDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        [BindProperty]
        public string AuctionId { get; set; }

        public async Task<IActionResult> OnGetAsync(string auctionId, string reason)
        {
            if (User.IsInRole("Administrator"))
            {
                var refuseResult = await AuctionRefused(auctionId);

                if (refuseResult)
                {
                    AuctionId = auctionId;

                    var ownerId = UserServices.GetAuctionOwner(_context, AuctionId);
                    var owner = await UserServices.GetUserById(_context, ownerId);

                    var currentUserId = await UserServices.GetCurrentUser(_context, User.Identity.Name);

                    if (ownerId == "" || ownerId == null || currentUserId == null || currentUserId == "")
                    {
                        return RedirectToPage("/NotFound");

                    }

                    var sendResult = await NotificationServices.Send(_context,
                        currentUserId,
                        ownerId,
                        DefaultNotificationMessages.LISTING_REFUSED_TITLE,
                        reason);

                    if (sendResult == false)
                    {
                        return RedirectToPage("/Error");
                    }

                    _emailService.SendEmail(new EmailDto()
                    {
                        To = owner.Email,
                        Subject = DefaultNotificationMessages.AUCTION_REFUSED_TITLE,
                        Body = reason
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

        public async Task<bool> AuctionRefused(string auctionId)
        {
            _context.Auctions.First(a => a.Id == auctionId).Status = ListingStatusTypes.Closed.ToString();
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
