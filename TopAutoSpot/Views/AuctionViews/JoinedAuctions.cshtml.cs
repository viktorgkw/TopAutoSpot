namespace TopAutoSpot.Views.AuctionViews
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;
    using NewsAPI.Models;

    using TopAutoSpot.Data;
    using TopAutoSpot.Data.Models;
    using TopAutoSpot.Data.Models.Enums;
    using TopAutoSpot.Services.NewsServices;

    /// <summary>
    /// This page model class displays a list of auctions that the current user has joined, and some news.
    /// </summary>
    [Authorize]
    public class JoinedAuctionsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly INewsService _newsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="JoinedAuctionsModel"/> class with the specified parameters.
        /// </summary>
        /// <param name="context">The application database context.</param>
        /// <param name="newsService">The news service.</param>
        public JoinedAuctionsModel(ApplicationDbContext context, INewsService newsService)
        {
            _context = context;
            _newsService = newsService;
        }

        /// <summary>
        /// Gets or sets the news items displayed on the page.
        /// </summary>
        public List<Article> News { get; set; } = null!;

        /// <summary>
        /// Gets or sets the auctions displayed on the page.
        /// </summary>
        public List<Auction> Auctions { get; set; } = null!;

        /// <summary>
        /// Handles HTTP GET requests for the page.
        /// </summary>
        /// <returns>The page result.</returns>
        public async Task<IActionResult> OnGetAsync()
        {
            News = await _newsService.GetNews(3);
            Auctions = _context.Auctions
                .AsNoTracking()
                .Where(a => a.Bidders!.Any(b => b.UserName == User.Identity!.Name))
                .Where(a => a.Status == AuctionStatusTypes.Active.ToString() ||
                            a.Status == AuctionStatusTypes.StartingSoon.ToString() ||
                            a.Status == AuctionStatusTypes.InProgress.ToString())
                .ToList();

            return Page();
        }

        /// <summary>
        /// Gets the base64 encoded data URL for the image of the specified auction.
        /// </summary>
        /// <param name="auctionId">The ID of the auction.</param>
        /// <returns>The data URL for the image.</returns>
        public string GetAuctionImage(string auctionId)
        {
            string carId = _context.Auctions
                .AsNoTracking()
                .First(a => a.Id == auctionId)
                .VehicleId;

            byte[] data = _context.VehicleImages
                .AsNoTracking()
                .First(i => i.VehicleId == carId)
                .ImageData;

            string imgDataURL = "data:image;base64," + Convert.ToBase64String(data);
            return imgDataURL;
        }
    }
}
