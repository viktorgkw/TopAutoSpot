using NewsAPI.Models;
using TopAutoSpot.Data;
using TopAutoSpot.Models;
using TopAutoSpot.Views.Utilities;
using TopAutoSpot.Models.Utilities;
using TopAutoSpot.Services.NewsServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace TopAutoSpot.Views.Buy
{
    [Authorize]
    public class BoatListingsModel : PageModel
    {
        private ApplicationDbContext _context;
        private INewsService _newsService;
        public BoatListingsModel(ApplicationDbContext db, INewsService newsService)
        {
            _context = db;
            _newsService = newsService;
        }

        [BindProperty]
        public string OrderSetting { get; set; }
        public List<Boat> Boats { get; set; }
        public List<Article> News = new List<Article>();

        public async Task<IActionResult> OnGetAsync(string orderSetting)
        {
            Boats = await _context.Boats
                .Where(b => b.Status == ListingStatusTypes.Active.ToString() && b.Price > 0)
                .ToListAsync();

            if (orderSetting != null)
            {
                Boats = VehicleCollectionSorter
                    .SortBy(Boats, orderSetting)
                    .ToList();
            }

            News = await _newsService.GetNews(3);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            return RedirectToPage("/Buy/BoatListings", new { orderSetting = OrderSetting });
        }

        public string GetImage(string boatId)
        {
            var data = _context.VehicleImages.First(i => i.VehicleId == boatId).ImageData;
            string imgDataURL = "data:image;base64," + Convert.ToBase64String(data);
            return imgDataURL;
        }

        public bool HasAnyImages(string boatId)
        {
            return _context.VehicleImages.Where(img => img.VehicleId == boatId).ToList().Count > 0;
        }

        public async Task<List<InterestedListing>> GetInterestedVehicles()
        {
            var currentUser = await _context.Users.FirstAsync(u => u.UserName == User.Identity.Name);

            return await _context.InterestedInListings.Where(l => l.UserId == currentUser.Id).ToListAsync();
        }
    }
}
