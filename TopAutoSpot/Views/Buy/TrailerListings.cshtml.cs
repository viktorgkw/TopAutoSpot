using Microsoft.AspNetCore.Mvc.RazorPages;
using TopAutoSpot.Data.Entities;
using TopAutoSpot.Data;
using Microsoft.EntityFrameworkCore;
using TopAutoSpot.Data.Entities.Utilities;
using Microsoft.AspNetCore.Mvc;
using TopAutoSpot.Views.Utilities;

namespace TopAutoSpot.Views.Buy
{
    public class TrailerListingsModel : PageModel
    {
        private ApplicationDbContext _context;
        public TrailerListingsModel(ApplicationDbContext db)
        {
            _context = db;
        }

        [BindProperty]
        public string OrderSetting { get; set; }
        public List<Trailer> Trailers { get; set; }

        public async Task<IActionResult> OnGetAsync(string orderSetting)
        {
            Trailers = await _context.Trailers
                .Where(t => t.Status == StatusTypes.Active.ToString())
                .ToListAsync();

            if (orderSetting != null)
            {
                Trailers = VehicleCollectionSorter
                    .SortBy(Trailers, orderSetting)
                    .ToList();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            return RedirectToPage("/Buy/TrailerListings", new { orderSetting = OrderSetting });
        }

        public string GetImage(string trailerId)
        {
            var data = _context.VehicleImages.First(i => i.VehicleId == trailerId).ImageData;
            string imgDataURL = "data:image;base64," + Convert.ToBase64String(data);
            return imgDataURL;
        }

        public bool HasAnyImages(string trailerId)
        {
            return _context.VehicleImages.Where(img => img.VehicleId == trailerId).ToList().Count > 0;
        }
    }
}
