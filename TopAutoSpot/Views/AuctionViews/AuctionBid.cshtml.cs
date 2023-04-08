namespace TopAutoSpot.Views.AuctionViews
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;

    using TopAutoSpot.Data;
    using TopAutoSpot.Data.Models;
    using TopAutoSpot.Data.Models.Enums;

    /// <summary>
    /// This class represents a PageModel for bidding on auctions, with authorization and property binding.
    /// </summary>
    [Authorize]
    [BindProperties]
    public class AuctionBidModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Constructor for AuctionBidModel, initializes the ApplicationDbContext.
        /// </summary>
        /// <param name="context">The ApplicationDbContext to use.</param>
        public AuctionBidModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public int OutbidAmount { get; set; }

        public Auction? Auction { get; set; }

        /// <summary>
        /// Handles HTTP GET requests to the AuctionBid page, retrieves the auction and verifies user authorization.
        /// </summary>
        /// <param name="id">The ID of the auction to retrieve.</param>
        /// <returns>An IActionResult representing the result of the request.</returns>
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

            if (!Auction.Bidders!.Any(u => u.Id == currentUser))
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

        /// <summary>
        /// Handles HTTP POST requests to the AuctionBid page, updates the auction with the new bid.
        /// </summary>
        /// <returns>An IActionResult representing the result of the request.</returns>
        public IActionResult OnPost()
        {
            Auction auctionToUpdateBid = _context.Auctions.First(a => a.Id == Auction!.Id);

            auctionToUpdateBid.LastBidderId = GetCurrentUserId();
            auctionToUpdateBid.CurrentBidPrice += OutbidAmount;

            _context.SaveChanges();

            OutbidAmount = 0;

            return RedirectToPage("/AuctionViews/AuctionBid", new { id = Auction!.Id });
        }

        /// <summary>
        /// Retrieves the image data for an auction.
        /// </summary>
        /// <param name="auctionId">The ID of the auction to retrieve the image for.</param>
        /// <returns>A Base64-encoded string representing the image data.</returns>
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

        /// <summary>
        /// Retrieves the username of the last bidder on an auction.
        /// </summary>
        /// <returns>The username of the last bidder.</returns>
        public string? GetLastBidderUsername()
        {
            var lastBidder = _context.Users
                .FirstOrDefault(u => u.Id == Auction!.LastBidderId);

            return lastBidder?.UserName;
        }

        /// <summary>
        /// Retrieves the ID of the current user.
        /// </summary>
        /// <returns>The ID of the current user.</returns>
        private string GetCurrentUserId()
        {
            return _context.Users
                .First(u => u.UserName == User.Identity!.Name)
                .Id;
        }
    }
}
