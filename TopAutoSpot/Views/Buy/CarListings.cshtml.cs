using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using TopAutoSpot.Data;
using TopAutoSpot.Data.Entities;
using TopAutoSpot.Data.Entities.Utilities;
using TopAutoSpot.Views.Utilities;

namespace TopAutoSpot.Views.Buy
{
    public class CarListingsModel : PageModel
    {
        private ApplicationDbContext _context;
        public CarListingsModel(ApplicationDbContext db)
        {
            _context = db;
        }

        [BindProperty]
        public string OrderSetting { get; set; }
        public List<Car> Cars { get; set; }

        public async Task<IActionResult> OnGetAsync(string orderSetting)
        {
            Cars = await _context.Cars
                        .Where(c => c.Status == StatusTypes.Active.ToString())
                        .ToListAsync();

            if (orderSetting != null)
            {
                Cars = VehicleCollectionSorter
                    .SortBy(Cars, orderSetting)
                    .ToList();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            return RedirectToPage("/Buy/CarListings", new { orderSetting = OrderSetting });
        }

        public string GetImage(string carId)
        {
            var data = _context.VehicleImages.First(i => i.VehicleId == carId).ImageData;
            string imgDataURL = "data:image;base64," + Convert.ToBase64String(data);
            return imgDataURL;
        }

        public bool HasAnyImages(string carId)
        {
            return _context.VehicleImages.Where(img => img.VehicleId == carId).ToList().Count > 0;
        }
    }
}
