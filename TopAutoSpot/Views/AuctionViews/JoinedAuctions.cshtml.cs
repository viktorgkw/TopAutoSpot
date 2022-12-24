using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NewsAPI.Models;
using TopAutoSpot.Data;
using TopAutoSpot.Models;
using TopAutoSpot.Models.Utilities;
using TopAutoSpot.Services.NewsServices;

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
            Auctions = _context.Auctions
                .Where(a => a.Bidders.Any(b => b.UserName == User.Identity.Name))
                .Where(a => a.Status == AuctionStatusTypes.Active.ToString() ||
                            a.Status == AuctionStatusTypes.StartingSoon.ToString() ||
                            a.Status == AuctionStatusTypes.InProgress.ToString())
                .ToList();

            return Page();
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
    }
}
