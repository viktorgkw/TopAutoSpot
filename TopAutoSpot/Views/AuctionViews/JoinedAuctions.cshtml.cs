using NewsAPI.Models;
using TopAutoSpot.Data;
using TopAutoSpot.Models;
using TopAutoSpot.Services.NewsServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using TopAutoSpot.Models.Utilities;

namespace TopAutoSpot.Views.AuctionViews
{
    [Authorize]
    public class JoinedAuctionsModel : PageModel
    {
        private ApplicationDbContext _context;
        private INewsService _newsService;
        public JoinedAuctionsModel(ApplicationDbContext context, INewsService newsService)
        {
            _context = context;
            _newsService = newsService;
        }

        public List<Article> News { get; set; }
        public List<Auction> Auctions { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            News = await _newsService.GetNews(3);
            Auctions = await _context.Auctions
                .Where(a => a.Bidders.Any(b => b.UserName == User.Identity.Name))
                .Where(a => a.Status == AuctionStatusTypes.Active.ToString() ||
                            a.Status == AuctionStatusTypes.StartingSoon.ToString() ||
                            a.Status == AuctionStatusTypes.InProgress.ToString())
                .ToListAsync();

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