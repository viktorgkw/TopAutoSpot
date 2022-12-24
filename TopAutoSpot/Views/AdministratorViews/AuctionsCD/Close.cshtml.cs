using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopAutoSpot.Data;
using TopAutoSpot.Models;
using TopAutoSpot.Models.Utilities;
using TopAutoSpot.Services.EmailService;
using TopAutoSpot.Views.Utilities;

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

        public IActionResult OnGet(string id)
        {
            if (User.IsInRole("Administrator"))
            {
                AuctionToClose = _context.Auctions.First(u => u.Id == id);
                AuctionToClose.Status = AuctionStatusTypes.Closed.ToString();
                _context.SaveChanges();

                User owner = UserServices.GetUserById(_context, AuctionToClose.AuctioneerId);
                string currentUser = UserServices.GetCurrentUser(_context, User.Identity.Name);

                NotificationServices.Send(_context,
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
