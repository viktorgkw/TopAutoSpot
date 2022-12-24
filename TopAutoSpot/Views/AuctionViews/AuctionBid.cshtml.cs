using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TopAutoSpot.Data;
using TopAutoSpot.Models;
using TopAutoSpot.Models.Utilities;

namespace TopAutoSpot.Views.AuctionViews
{
    [Authorize]
    [BindProperties]
    public class AuctionBidModel : PageModel
    {
        private ApplicationDbContext _context;
        public AuctionBidModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public int OutbidAmount { get; set; }
        public Auction Auction { get; set; }

        public IActionResult OnGet(string id)
        {
            Auction = _context.Auctions.FirstOrDefault(a => a.Id == id);
            string currentUser = GetCurrentUserId();

            if (Auction == null)
            {
                return RedirectToPage("/NotFound");
            }

            if (currentUser == null)
            {
                return RedirectToPage("/NotFound");
            }

            if (!Auction.Bidders.Any(u => u.Id == currentUser))
            {
                return RedirectToPage("/NotFound");
            }

            string[] validStatuses = new string[] { AuctionStatusTypes.StartingSoon.ToString(),
                                            AuctionStatusTypes.InProgress.ToString(),
                                            AuctionStatusTypes.Ended.ToString()};

            if (!validStatuses.Contains(Auction.Status))
            {
                return RedirectToPage("/NotFound");
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            Auction auctionToUpdateBid = _context.Auctions.First(a => a.Id == Auction.Id);

            auctionToUpdateBid.LastBidderId = GetCurrentUserId();
            auctionToUpdateBid.CurrentBidPrice += OutbidAmount;

            _context.SaveChanges();

            OutbidAmount = 0;

            return RedirectToPage("/AuctionViews/AuctionBid", new { id = Auction.Id });
        }

        public string GetAuctionImage(string auctionId)
        {
            string carId = _context.Auctions
                .First(a => a.Id == auctionId)
                .VehicleId;

            byte[] data = _context.VehicleImages
                .First(i => i.VehicleId == carId)
                .ImageData;

            string imgDataURL = "data:image;base64," + Convert.ToBase64String(data);
            return imgDataURL;
        }

        public string GetLastBidderUsername()
        {
            return _context.Users
                .First(u => u.Id == Auction.LastBidderId)
                .UserName;
        }

        private string GetCurrentUserId()
        {
            return _context.Users
                .First(u => u.UserName == User.Identity.Name)
                .Id;
        }
    }
}
