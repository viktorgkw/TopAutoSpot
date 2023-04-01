using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NewsAPI.Models;
using TopAutoSpot.Data;
using TopAutoSpot.Data.Models;
using TopAutoSpot.Data.Models.Enums;
using TopAutoSpot.Services.NewsServices;
using TopAutoSpot.Services.Utilities;

namespace TopAutoSpot.Views.Buy
{
    [Authorize]
    public class MotorcycleListingsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        private readonly INewsService _newsService;

        public MotorcycleListingsModel(ApplicationDbContext db, INewsService newsService)
        {
            _context = db;
            _newsService = newsService;
        }

        [BindProperty]
        public string? OrderSetting { get; set; }

        public List<Motorcycle> Motorcycles { get; set; } = null!;

        public List<Article> News = new();

        public async Task<IActionResult> OnGetAsync(string orderSetting)
        {
            Motorcycles = _context.Motorcycles
                .AsNoTracking()
                .Where(m => m.Status == ListingStatusTypes.Active.ToString() && m.Price > 0)
                .ToList();

            if (orderSetting != null)
            {
                Motorcycles = VehicleCollectionSorter
                    .SortBy(Motorcycles, orderSetting)
                    .ToList();
            }

            News = await _newsService.GetNews(3);

            return Page();
        }

        public IActionResult OnPost()
        {
            return RedirectToPage("/Buy/MotorcycleListings", new { orderSetting = OrderSetting });
        }

        public string GetImage(string motorcycleId)
        {
            byte[] data = _context.VehicleImages
                .AsNoTracking()
                .First(i => i.VehicleId == motorcycleId)
                .ImageData;

            string imgDataURL = "data:image;base64," + Convert.ToBase64String(data);
            return imgDataURL;
        }

        public bool HasAnyImages(string motorcycleId)
        {
            return _context.VehicleImages
                .AsNoTracking()
                .Where(img => img.VehicleId == motorcycleId)
                .ToList().Count > 0;
        }

        public List<InterestedListing> GetInterestedVehicles()
        {
            User currentUser = _context.Users
                .AsNoTracking()
                .First(u => u.UserName == User.Identity!.Name);

            return _context.InterestedInListings
                .AsNoTracking()
                .Where(l => l.UserId == currentUser.Id)
                .ToList();
        }
    }
}
