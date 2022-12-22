using TopAutoSpot.Data;
using TopAutoSpot.Models;
using TopAutoSpot.Views.Utilities;
using TopAutoSpot.Services.EmailService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

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

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (User.IsInRole("Administrator"))
            {
                AuctionToDelete = await _context.Auctions.FirstAsync(u => u.Id == id);
                var owner = await UserServices.GetUserById(_context, AuctionToDelete.AuctioneerId);
                var currentUser = await UserServices.GetCurrentUser(_context, User.Identity.Name);

                _context.Auctions.Remove(AuctionToDelete);
                await _context.SaveChangesAsync();

                await NotificationServices.Send(_context,
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
