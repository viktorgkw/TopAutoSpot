using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopAutoSpot.Data;
using TopAutoSpot.Data.Models;
using TopAutoSpot.Services.EmailService;
using TopAutoSpot.Views.Utilities;

namespace TopAutoSpot.Views.AdministratorViews.AuctionsCD
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private ApplicationDbContext _context;
        private IEmailService _emailService;

        public DeleteModel(ApplicationDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        [BindProperty]
        public Auction AuctionToDelete { get; set; }

        public IActionResult OnGet(string id)
        {
            if (User.IsInRole("Administrator"))
            {
                AuctionToDelete = _context.Auctions.First(u => u.Id == id);
                User owner = _context.Users.First(u => u.Id == AuctionToDelete.AuctioneerId);
                string currentUser = UserServices.GetCurrentUser(_context, User.Identity.Name);

                _context.Auctions.Remove(AuctionToDelete);
                _context.SaveChanges();

                NotificationServices.Send(_context,
                    currentUser,
                    owner.Id,
                    DefaultNotificationMessages.AUCTION_DELETED_TITLE,
                    DefaultNotificationMessages.AUCTION_DELETED_DESCRIPTION);

                _emailService.SendEmail(new EmailDto()
                {
                    To = owner.Email,
                    Subject = DefaultNotificationMessages.AUCTION_DELETED_TITLE,
                    Body = DefaultNotificationMessages.AUCTION_DELETED_DESCRIPTION
                });

                return Page();
            }

            return RedirectToPage("/NotFound");
        }
    }
}
