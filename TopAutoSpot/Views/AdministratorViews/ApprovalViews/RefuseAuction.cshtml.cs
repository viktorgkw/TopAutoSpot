using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopAutoSpot.Data;
using TopAutoSpot.Data.Models;
using TopAutoSpot.Data.Models.Enums;
using TopAutoSpot.Services.EmailService;
using TopAutoSpot.Views.Utilities;

namespace TopAutoSpot.Views.AdministratorViews.ApprovalViews
{
    [Authorize]
    public class RefuseAuctionModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        private readonly IEmailService _emailService;

        public RefuseAuctionModel(ApplicationDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        [BindProperty]
        public string AuctionId { get; set; } = null!;

        public IActionResult OnGet(string auctionId, string reason)
        {
            if (User.IsInRole("Administrator"))
            {
                bool refuseResult = AuctionRefused(auctionId);

                if (refuseResult)
                {
                    AuctionId = auctionId;

                    string ownerId = UserServices.GetAuctionOwner(_context, AuctionId);
                    User owner = UserServices.GetUserById(_context, ownerId);

                    string currentUserId = UserServices.GetCurrentUser(_context, User.Identity!.Name!);

                    if (ownerId == "" || ownerId == null || currentUserId == null || currentUserId == "")
                    {
                        return RedirectToPage("/NotFound");

                    }

                    bool sendResult = NotificationServices.Send(_context,
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
                        To = owner.Email!,
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

        public bool AuctionRefused(string auctionId)
        {
            _context.Auctions.First(a => a.Id == auctionId).Status = ListingStatusTypes.Closed.ToString();
            _context.SaveChanges();

            return true;
        }
    }
}
