using Microsoft.AspNetCore.Mvc.RazorPages;
using TopAutoSpot.Data.Entities;
using TopAutoSpot.Data;
using Microsoft.EntityFrameworkCore;
using TopAutoSpot.Data.Entities.Utilities;
using Microsoft.AspNetCore.Mvc;
using TopAutoSpot.Views.Utilities;

namespace TopAutoSpot.Views.Buy
{
    public class BoatListingsModel : PageModel
    {
        private ApplicationDbContext _context;
        public BoatListingsModel(ApplicationDbContext db)
        {
            _context = db;
        }

        [BindProperty]
        public string OrderSetting { get; set; }
        public List<Boat> Boats { get; set; }

        public async Task<IActionResult> OnGetAsync(string orderSetting)
        {
            Boats = await _context.Boats
                .Where(b => b.Status == StatusTypes.Active.ToString() && b.Price > 0)
                .ToListAsync();

            if (orderSetting != null)
            {
                Boats = VehicleCollectionSorter
                    .SortBy(Boats, orderSetting)
                    .ToList();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            return RedirectToPage("/Buy/BoatListings", new { orderSetting = OrderSetting });
        }

        public string GetImage(string boatId)
        {
            var data = _context.VehicleImages.First(i => i.VehicleId == boatId).ImageData;
            string imgDataURL = "data:image;base64," + Convert.ToBase64String(data);
            return imgDataURL;
        }

        public bool HasAnyImages(string boatId)
        {
            return _context.VehicleImages.Where(img => img.VehicleId == boatId).ToList().Count > 0;
        }

        public async Task<List<InterestedListing>> GetInterestedVehicles()
        {
            var currentUser = await _context.Users.FirstAsync(u => u.UserName == User.Identity.Name);

            return await _context.InterestedInListings.Where(l => l.UserId == currentUser.Id).ToListAsync();
        }
    }
}
