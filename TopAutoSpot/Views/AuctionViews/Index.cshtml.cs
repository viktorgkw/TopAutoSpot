using NewsAPI.Models;
using TopAutoSpot.Data;
using TopAutoSpot.Models;
using TopAutoSpot.Models.Utilities;
using TopAutoSpot.Services.NewsServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace TopAutoSpot.Views.AuctionViews
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private ApplicationDbContext _context;
        private INewsService _newsService;
        public IndexModel(ApplicationDbContext context, INewsService newsService)
        {
            _context = context;
            _newsService = newsService;
        }

        public List<Auction> StartingSoonAuctions = new List<Auction>();
        public List<Auction> ActiveAuctions = new List<Auction>();
        public List<Article> News = new List<Article>();
        public int OverallAuctions { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            News = await _newsService.GetNews(3);

            StartingSoonAuctions = await _context.Auctions
                .Where(a => a.Status == AuctionStatusTypes.StartingSoon.ToString())
                .ToListAsync();

            ActiveAuctions = await _context.Auctions
                .Where(a => a.Status == AuctionStatusTypes.Active.ToString())
                .ToListAsync();

            OverallAuctions = StartingSoonAuctions.Count + ActiveAuctions.Count;

            return Page();
        }

        public string GetAuctionImage(string auctionId)
        {
            var carId = _context.Auctions.First(a => a.Id == auctionId).VehicleId;
            var data = _context.VehicleImages.First(i => i.VehicleId == carId).ImageData;
            string imgDataURL = "data:image;base64," + Convert.ToBase64String(data);
            return imgDataURL;
        }
    }
}
