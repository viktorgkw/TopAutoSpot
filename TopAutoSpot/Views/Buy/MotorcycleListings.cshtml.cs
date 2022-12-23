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
    public class MotorcycleListingsModel : PageModel
    {
        private ApplicationDbContext _context;
        private INewsService _newsService;
        public MotorcycleListingsModel(ApplicationDbContext db, INewsService newsService)
        {
            _context = db;
            _newsService = newsService;
        }

        [BindProperty]
        public string OrderSetting { get; set; }
        public List<Motorcycle> Motorcycles { get; set; }
        public List<Article> News = new List<Article>();

        public async Task<IActionResult> OnGetAsync(string orderSetting)
        {
            Motorcycles = await _context.Motorcycles
                .Where(m => m.Status == ListingStatusTypes.Active.ToString() && m.Price > 0)
                .ToListAsync();

            if (orderSetting != null)
            {
                Motorcycles = VehicleCollectionSorter
                    .SortBy(Motorcycles, orderSetting)
                    .ToList();
            }

            News = await _newsService.GetNews(3);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            return RedirectToPage("/Buy/MotorcycleListings", new { orderSetting = OrderSetting });
        }

        public string GetImage(string motorcycleId)
        {
            var data = _context.VehicleImages.First(i => i.VehicleId == motorcycleId).ImageData;
            string imgDataURL = "data:image;base64," + Convert.ToBase64String(data);
            return imgDataURL;
        }

        public bool HasAnyImages(string motorcycleId)
        {
            return _context.VehicleImages.Where(img => img.VehicleId == motorcycleId).ToList().Count > 0;
        }

        public async Task<List<InterestedListing>> GetInterestedVehicles()
        {
            var currentUser = await _context.Users.FirstAsync(u => u.UserName == User.Identity.Name);

            return await _context.InterestedInListings.Where(l => l.UserId == currentUser.Id).ToListAsync();
        }
    }
}
