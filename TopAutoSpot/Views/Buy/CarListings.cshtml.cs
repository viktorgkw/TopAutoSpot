using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TopAutoSpot.Data;
using TopAutoSpot.Models;
using TopAutoSpot.Models.Utilities;
using TopAutoSpot.Views.Utilities;

namespace TopAutoSpot.Views.Buy
{
    [Authorize]
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
                        .Where(c => c.Status == StatusTypes.Active.ToString() && c.Price > 0)
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

        public async Task<List<InterestedListing>> GetInterestedVehicles()
        {
            var currentUser = await _context.Users.FirstAsync(u => u.UserName == User.Identity.Name);

            return await _context.InterestedInListings.Where(l => l.UserId == currentUser.Id).ToListAsync();
        }
    }
}
