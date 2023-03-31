﻿using TopAutoSpot.Data;
using TopAutoSpot.Data.Models;
using TopAutoSpot.Data.Models.Enums;
using TopAutoSpot.Services.Common;
using TopAutoSpot.Services.EmailServices;

namespace TopAutoSpot.Services.AuctionServices
{
    public class AuctionService : IAuctionService
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;

        public AuctionService(ApplicationDbContext context, IEmailService emailSerice)
        {
            _context = context;
            _emailService = emailSerice;
        }

        public Task StartingAuctionsCheck()
        {
            List<Auction> auctionsToday = _context.Auctions
                .Where(a => a.StartDay.Day == DateTime.Now.Day)
                .ToList();

            if (auctionsToday.Count == 0)
            {
                return Task.FromResult(0);
            }

            foreach (Auction? auction in auctionsToday)
            {
                if (auction.StartHour.Hour == DateTime.Now.Hour)
                {
                    auction.Status = AuctionStatusTypes.InProgress.ToString();
                    _context.SaveChanges();
                }
                else
                {
                    if (auction.StartHour.AddHours(auction.Duration).Hour == DateTime.Now.Hour)
                    {
                        auction.Status = AuctionStatusTypes.Ended.ToString();
                        _context.SaveChanges();

                        User winner = _context.Users.First(u => u.Id == auction.AuctioneerId);

                        _emailService.SendEmail(new EmailDto()
                        {
                            To = winner.Email!,
                            Subject = DefaultNotificationMessages.AUCTION_WINNER_TITLE,
                            Body = string.Format(DefaultNotificationMessages.AUCTION_WINNER_DESCRIPTION,
                                auction.Title)
                        });
                    }
                }
            }

            return Task.FromResult(0);
        }

        public Task DailyCheckAndRemind()
        {
            List<Auction> auctionsToday = _context.Auctions
                .Where(a => a.StartDay.Day == DateTime.Now.Day)
                .ToList();

            foreach (Auction? auction in auctionsToday)
            {
                if (auction.Bidders!.Count < 3)
                {
                    auction.Status = AuctionStatusTypes.Closed.ToString();
                    _context.SaveChanges();
                    continue;
                }
                else
                {
                    auction.Status = AuctionStatusTypes.StartingSoon.ToString();
                    _context.SaveChanges();
                }

                foreach (User bidder in auction.Bidders)
                {
                    _emailService.SendEmail(new EmailDto()
                    {
                        To = bidder.Email!,
                        Subject = DefaultNotificationMessages.AUCTION_REMINDER_TITLE,
                        Body = string.Format(DefaultNotificationMessages.AUCTION_REMINDER_DESCRIPTION, auction.Title)
                    });
                }
            }

            return Task.FromResult(0);
        }
    }
}