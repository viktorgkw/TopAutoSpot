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
    /// The IndexModel class represents the code-behind for the Index Razor Page, which is responsible
    /// for displaying information about auctions and news on the application's home page.
    /// </summary>
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly INewsService _newsService;

        /// <summary>
        /// Initializes a new instance of the <see cref="IndexModel"/> class.
        /// </summary>
        /// <param name="context">An instance of the <see cref="ApplicationDbContext"/> class.</param>
        /// <param name="newsService">An instance of the <see cref="INewsService"/> interface.</param>
        public IndexModel(ApplicationDbContext context, INewsService newsService)
        {
            _context = context;
            _newsService = newsService;
        }

        /// <summary>
        /// Gets or sets the current user.
        /// </summary>
        public User CurrentUser { get; set; } = null!;

        /// <summary>
        /// Gets or sets the list of starting soon auctions for the current user.
        /// </summary>
        public List<Auction> StartingSoonAuctionsForCurrentUser = new();

        /// <summary>
        /// Gets or sets the list of active auctions.
        /// </summary>
        public List<Auction> ActiveAuctions = new();

        /// <summary>
        /// Gets or sets the list of news articles.
        /// </summary>
        public List<Article> News = new();

        /// <summary>
        /// Gets or sets the overall number of auctions.
        /// </summary>
        public int OverallAuctions { get; set; }

        /// <summary>
        /// Retrieves data for display on the page.
        /// </summary>
        /// <returns>An instance of <see cref="IActionResult"/>.</returns>
        public async Task<IActionResult> OnGetAsync()
        {
            CurrentUser = _context.Users
                .AsNoTracking()
                .First(u => u.UserName == User.Identity!.Name);

            News = await _newsService.GetNews(3);

            StartingSoonAuctionsForCurrentUser = _context.Auctions
                .AsNoTracking()
                .Where(a => a.Bidders!.Any(b => b.UserName == CurrentUser.UserName))
                .Where(a => a.Status == AuctionStatusTypes.StartingSoon.ToString() ||
                            a.Status == AuctionStatusTypes.InProgress.ToString())
                .ToList();

            ActiveAuctions = _context.Auctions
                .AsNoTracking()
                .Where(a => a.Status == AuctionStatusTypes.Active.ToString())
                .ToList();

            OverallAuctions = StartingSoonAuctionsForCurrentUser.Count + ActiveAuctions.Count;

            return Page();
        }

        /// <summary>
        /// Gets the image data URL for an auction.
        /// </summary>
        /// <param name="auctionId">The ID of the auction.</param>
        /// <returns>A string representing the image data URL.</returns>
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
