using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NewsAPI.Models;
using TopAutoSpot.Data;
using TopAutoSpot.Data.Models;
using TopAutoSpot.Data.Models.Enums;
using TopAutoSpot.Services.NewsServices;
using TopAutoSpot.Views.Utilities;

namespace TopAutoSpot.Views.Buy
{
    [Authorize]
    public class TrailerListingsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly INewsService _newsService;
        public TrailerListingsModel(ApplicationDbContext db, INewsService newsService)
        {
            _context = db;
            _newsService = newsService;
        }

        [BindProperty]
        public string? OrderSetting { get; set; }
        public List<Trailer> Trailers { get; set; } = null!;
        public List<Article> News = new();

        public async Task<IActionResult> OnGetAsync(string orderSetting)
        {
            Trailers = _context.Trailers
                .AsNoTracking()
                .Where(t => t.Status == ListingStatusTypes.Active.ToString() && t.Price > 0)
                .ToList();

            if (orderSetting != null)
            {
                Trailers = VehicleCollectionSorter
                    .SortBy(Trailers, orderSetting)
                    .ToList();
            }

            News = await _newsService.GetNews(3);

            return Page();
        }

        public IActionResult OnPost()
        {
            return RedirectToPage("/Buy/TrailerListings", new { orderSetting = OrderSetting });
        }

        public string GetImage(string trailerId)
        {
            byte[] data = _context.VehicleImages
                .AsNoTracking()
                .First(i => i.VehicleId == trailerId)
                .ImageData;

            string imgDataURL = "data:image;base64," + Convert.ToBase64String(data);
            return imgDataURL;
        }

        public bool HasAnyImages(string trailerId)
        {
            return _context.VehicleImages
                .AsNoTracking()
                .Where(img => img.VehicleId == trailerId)
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
