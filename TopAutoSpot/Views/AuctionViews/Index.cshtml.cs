using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NewsAPI.Models;
using TopAutoSpot.Data;
using TopAutoSpot.Data.Models;
using TopAutoSpot.Data.Models.Enums;
using TopAutoSpot.Services.NewsServices;

namespace TopAutoSpot.Views.AuctionViews
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        private readonly INewsService _newsService;

        public IndexModel(ApplicationDbContext context, INewsService newsService)
        {
            _context = context;
            _newsService = newsService;
        }

        public User CurrentUser { get; set; } = null!;

        public List<Auction> StartingSoonAuctionsForCurrentUser = new();

        public List<Auction> ActiveAuctions = new();

        public List<Article> News = new();

        public int OverallAuctions { get; set; }

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
