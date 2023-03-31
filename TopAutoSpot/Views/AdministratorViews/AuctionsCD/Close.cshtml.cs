using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TopAutoSpot.Data;
using TopAutoSpot.Data.Models;
using TopAutoSpot.Data.Models.Enums;
using TopAutoSpot.Services.Common;
using TopAutoSpot.Services.EmailServices;
using TopAutoSpot.Views.Utilities;

namespace TopAutoSpot.Views.AdministratorViews.AuctionsCD
{
    [Authorize]
    public class CloseModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        private readonly IEmailService _emailService;

        public CloseModel(ApplicationDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        [BindProperty]
        public Auction AuctionToClose { get; set; } = null!;

        public IActionResult OnGet(string id)
        {
            if (User.IsInRole("Administrator"))
            {
                _context.Auctions.First(u => u.Id == id).Status = AuctionStatusTypes.Closed.ToString();
                _context.SaveChanges();

                User owner = UserServices.GetUserById(_context, AuctionToClose.AuctioneerId!);
                string currentUser = UserServices.GetCurrentUser(_context, User.Identity!.Name!);

                NotificationServices.Send(_context,
                    currentUser,
                    owner.Id,
                    DefaultNotificationMessages.AUCTION_CLOSED_TITLE,
                    DefaultNotificationMessages.AUCTION_CLOSED_DESCRIPTION);

                _emailService.SendEmail(new EmailDto()
                {
                    To = owner.Email!,
                    Subject = DefaultNotificationMessages.AUCTION_CLOSED_TITLE,
                    Body = DefaultNotificationMessages.AUCTION_CLOSED_DESCRIPTION
                });

                return Page();
            }

            return RedirectToPage("/NotFound");
        }
    }
}
