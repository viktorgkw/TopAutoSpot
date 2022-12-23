using TopAutoSpot.Data;
using TopAutoSpot.Models;
using TopAutoSpot.Models.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace TopAutoSpot.Views.VehiclePreview
{
    public class AuctionPreviewModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public AuctionPreviewModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Auction Auction { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null || _context.Auctions == null)
            {
                return RedirectToPage("/NotFound");
            }

            var auction = await _context.Auctions.FirstOrDefaultAsync(b => b.Id == id);
            var foundUser = await _context.Users.FirstAsync(u => u.UserName == User.Identity.Name);

            if (auction == null)
            {
                return RedirectToPage("/NotFound");
            }
            else if (auction.Status != ListingStatusTypes.Active.ToString() && auction.AuctioneerId != foundUser.Id)
            {
                return RedirectToPage("/MyVehicles/Index");
            }
            else
            {
                Auction = auction;
            }

            return Page();
        }

        public string GetOwnerFullName()
        {
            var foundUser = _context.Users
                .First(u => u.Id == Auction.AuctioneerId);

            return foundUser.FirstName + " " + foundUser.LastName;
        }

        public string GetImage()
        {
            var data = _context.VehicleImages.Where(img => img.VehicleId == Auction.VehicleId).First().ImageData;
            string imgDataURL = "data:image;base64," + Convert.ToBase64String(data);
            return imgDataURL;
        }
    }
}
