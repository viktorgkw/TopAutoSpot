using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using NewsAPI.Models;
using TopAutoSpot.Data;
using TopAutoSpot.Models;
using TopAutoSpot.Models.Utilities;
using TopAutoSpot.Services.NewsServices;
using TopAutoSpot.Views.Utilities;

namespace TopAutoSpot.Views.Buy
{
    [Authorize]
    public class BusListingsModel : PageModel
    {
        private ApplicationDbContext _context;
        private INewsService _newsService;
        public BusListingsModel(ApplicationDbContext db, INewsService newsService)
        {
            _context = db;
            _newsService = newsService;
        }

        [BindProperty]
        public string OrderSetting { get; set; }
        public List<Bus> Buses { get; set; }
        public List<Article> News = new List<Article>();

        public async Task<IActionResult> OnGetAsync(string orderSetting)
        {
            Buses = _context.Buses
                .Where(b => b.Status == ListingStatusTypes.Active.ToString() && b.Price > 0)
                .ToList();

            if (orderSetting != null)
            {
                Buses = VehicleCollectionSorter
                    .SortBy(Buses, orderSetting)
                    .ToList();
            }

            News = await _newsService.GetNews(3);

            return Page();
        }

        public IActionResult OnPost()
        {
            return RedirectToPage("/Buy/BusListings", new { orderSetting = OrderSetting });
        }

        public string GetImage(string busId)
        {
            byte[] data = _context.VehicleImages
                .First(i => i.VehicleId == busId)
                .ImageData;

            string imgDataURL = "data:image;base64," + Convert.ToBase64String(data);
            return imgDataURL;
        }

        public bool HasAnyImages(string busId)
        {
            return _context.VehicleImages
                .Where(img => img.VehicleId == busId)
                .ToList().Count > 0;
        }

        public List<InterestedListing> GetInterestedVehicles()
        {
            User currentUser = _context.Users.First(u => u.UserName == User.Identity.Name);

            return _context.InterestedInListings
                .Where(l => l.UserId == currentUser.Id)
                .ToList();
        }
    }
}
