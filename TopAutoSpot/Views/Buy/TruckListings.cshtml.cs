using Microsoft.AspNetCore.Mvc.RazorPages;
using TopAutoSpot.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using TopAutoSpot.Views.Utilities;
using TopAutoSpot.Models.Utilities;
using TopAutoSpot.Models;
using Microsoft.AspNetCore.Authorization;

namespace TopAutoSpot.Views.Buy
{
    [Authorize]
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
                .Where(t => t.Status == StatusTypes.Active.ToString() && t.Price > 0)
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

        public async Task<List<InterestedListing>> GetInterestedVehicles()
        {
            var currentUser = await _context.Users.FirstAsync(u => u.UserName == User.Identity.Name);

            return await _context.InterestedInListings.Where(l => l.UserId == currentUser.Id).ToListAsync();
        }
    }
}
