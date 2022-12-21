using Microsoft.AspNetCore.Mvc.RazorPages;
using TopAutoSpot.Data.Entities;
using TopAutoSpot.Data;
using Microsoft.EntityFrameworkCore;
using TopAutoSpot.Data.Entities.Utilities;
using Microsoft.AspNetCore.Mvc;
using TopAutoSpot.Views.Utilities;

namespace TopAutoSpot.Views.Buy
{
    public class BusListingsModel : PageModel
    {
        private ApplicationDbContext _context;
        public BusListingsModel(ApplicationDbContext db)
        {
            _context = db;
        }

        [BindProperty]
        public string OrderSetting { get; set; }
        public List<Bus> Buses { get; set; }

        public async Task<IActionResult> OnGetAsync(string orderSetting)
        {
            Buses = await _context.Buses
                .Where(b => b.Status == StatusTypes.Active.ToString())
                .ToListAsync();

            if (orderSetting != null)
            {
                Buses = VehicleCollectionSorter
                    .SortBy(Buses, orderSetting)
                    .ToList();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            return RedirectToPage("/Buy/BusListings", new { orderSetting = OrderSetting });
        }

        public string GetImage(string busId)
        {
            var data = _context.VehicleImages.First(i => i.VehicleId == busId).ImageData;
            string imgDataURL = "data:image;base64," + Convert.ToBase64String(data);
            return imgDataURL;
        }

        public bool HasAnyImages(string busId)
        {
            return _context.VehicleImages.Where(img => img.VehicleId == busId).ToList().Count > 0;
        }
    }
}
