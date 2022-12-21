using Microsoft.AspNetCore.Mvc.RazorPages;
using TopAutoSpot.Data.Entities;
using TopAutoSpot.Data;
using Microsoft.EntityFrameworkCore;
using TopAutoSpot.Data.Entities.Utilities;
using Microsoft.AspNetCore.Mvc;
using TopAutoSpot.Views.Utilities;

namespace TopAutoSpot.Views.Buy
{
    public class TruckListingsModel : PageModel
    {
        private ApplicationDbContext _context;
        public TruckListingsModel(ApplicationDbContext db)
        {
            _context = db;
        }

        [BindProperty]
        public string OrderSetting { get; set; }
        public List<Truck> Trucks { get; set; }

        public async Task<IActionResult> OnGetAsync(string orderSetting)
        {
            Trucks = await _context.Trucks
                .Where(t => t.Status == StatusTypes.Active.ToString())
                .ToListAsync();

            if (orderSetting != null)
            {
                Trucks = VehicleCollectionSorter
                    .SortBy(Trucks, orderSetting)
                    .ToList();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            return RedirectToPage("/Buy/TruckListings", new { orderSetting = OrderSetting });
        }

        public string GetImage(string truckId)
        {
            var data = _context.VehicleImages.First(i => i.VehicleId == truckId).ImageData;
            string imgDataURL = "data:image;base64," + Convert.ToBase64String(data);
            return imgDataURL;
        }

        public bool HasAnyImages(string truckId)
        {
            return _context.VehicleImages.Where(img => img.VehicleId == truckId).ToList().Count > 0;
        }
    }
}
