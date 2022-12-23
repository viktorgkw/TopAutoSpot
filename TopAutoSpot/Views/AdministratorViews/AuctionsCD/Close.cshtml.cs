using TopAutoSpot.Data;
using TopAutoSpot.Models;
using TopAutoSpot.Views.Utilities;
using TopAutoSpot.Models.Utilities;
using TopAutoSpot.Services.EmailService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace TopAutoSpot.Views.AdministratorViews.AuctionsCD
{
    [Authorize]
    public class CloseModel : PageModel
    {
        private ApplicationDbContext _context;
        private IEmailService _emailService;

        public CloseModel(ApplicationDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        [BindProperty]
        public Auction AuctionToClose { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (User.IsInRole("Administrator"))
            {
                AuctionToClose = await _context.Auctions.FirstAsync(u => u.Id == id);
                AuctionToClose.Status = AuctionStatusTypes.Closed.ToString();
                await _context.SaveChangesAsync();

                var owner = await UserServices.GetUserById(_context, AuctionToClose.AuctioneerId);
                var currentUser = await UserServices.GetCurrentUser(_context, User.Identity.Name);

                await NotificationServices.Send(_context,
                    currentUser,
                    owner.Id,
                    DefaultNotificationMessages.AUCTION_CLOSED_TITLE,
                    DefaultNotificationMessages.AUCTION_CLOSED_DESCRIPTION);

                _emailService.SendEmail(new EmailDto()
                {
                    To = owner.Email,
                    Subject = DefaultNotificationMessages.AUCTION_CLOSED_TITLE,
                    Body = DefaultNotificationMessages.AUCTION_CLOSED_DESCRIPTION
                });

                return Page();
            }

            return RedirectToPage("/NotFound");
        }
    }
}
