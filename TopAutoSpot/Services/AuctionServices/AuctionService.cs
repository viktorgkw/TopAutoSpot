using TopAutoSpot.Data;
using TopAutoSpot.Models;
using TopAutoSpot.Services.EmailService;
using Microsoft.EntityFrameworkCore;
using TopAutoSpot.Models.Utilities;
using TopAutoSpot.Views.Utilities;

namespace TopAutoSpot.Services.AuctionServices
{
    public class AuctionService : IAuctionService
    {
        private ApplicationDbContext _context;
        private IEmailService _emailService;
        public AuctionService(ApplicationDbContext context, IEmailService emailSerice)
        {
            _context = context;
            _emailService = emailSerice;
        }

        public async void DailyCheckAndRemind()
        {
            var auctionsToday = await _context.Auctions
                .Where(a => a.StartDay.Day == DateTime.Now.Day)
                .ToListAsync();

            foreach (var auction in auctionsToday)
            {
                if (auction.Bidders.Count < 3)
                {
                    auction.Status = AuctionStatusTypes.Closed.ToString();
                    await _context.SaveChangesAsync();
                    continue;
                }
                else
                {
                    auction.Status = AuctionStatusTypes.StartingSoon.ToString();
                    await _context.SaveChangesAsync();
                }

                foreach (var bidder in auction.Bidders)
                {
                    _emailService.SendEmail(new EmailDto()
                    {
                        To = bidder.Email,
                        Subject = DefaultNotificationMessages.AUCTION_REMINDER_TITLE,
                        Body = string.Format(DefaultNotificationMessages.AUCTION_REMINDER_DESCRIPTION, auction.Title)
                    });
                }
            }
        }
    }
}
