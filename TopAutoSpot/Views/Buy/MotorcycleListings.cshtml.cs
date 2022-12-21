using Microsoft.AspNetCore.Mvc.RazorPages;
using TopAutoSpot.Data.Entities;
using TopAutoSpot.Data;
using Microsoft.EntityFrameworkCore;
using TopAutoSpot.Data.Entities.Utilities;
using Microsoft.AspNetCore.Mvc;
using TopAutoSpot.Views.Utilities;

namespace TopAutoSpot.Views.Buy
{
    public class MotorcycleListingsModel : PageModel
    {
        private ApplicationDbContext _context;
        public MotorcycleListingsModel(ApplicationDbContext db)
        {
            _context = db;
        }

        [BindProperty]
        public string OrderSetting { get; set; }
        public List<Motorcycle> Motorcycles { get; set; }

        public async Task<IActionResult> OnGetAsync(string orderSetting)
        {
            Motorcycles = await _context.Motorcycles
                .Where(m => m.Status == StatusTypes.Active.ToString() && m.Price > 0)
                .ToListAsync();

            if (orderSetting != null)
            {
                Motorcycles = VehicleCollectionSorter
                    .SortBy(Motorcycles, orderSetting)
                    .ToList();
            }

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
