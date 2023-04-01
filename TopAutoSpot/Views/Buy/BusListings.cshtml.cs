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
    public class BusListingsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        private readonly INewsService _newsService;

        public BusListingsModel(ApplicationDbContext db, INewsService newsService)
        {
            _context = db;
            _newsService = newsService;
        }

        [BindProperty]
        public string? OrderSetting { get; set; }

        public List<Bus> Buses { get; set; } = null!;

        public List<Article> News = new();

        public async Task<IActionResult> OnGetAsync(string orderSetting)
        {
            Buses = _context.Buses
                .AsNoTracking()
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
                .AsNoTracking()
                .First(i => i.VehicleId == busId)
                .ImageData;

            string imgDataURL = "data:image;base64," + Convert.ToBase64String(data);
            return imgDataURL;
        }

        public bool HasAnyImages(string busId)
        {
            return _context.VehicleImages
                .AsNoTracking()
                .Where(img => img.VehicleId == busId)
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
