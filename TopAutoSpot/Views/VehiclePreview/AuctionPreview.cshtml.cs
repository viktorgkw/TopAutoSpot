using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TopAutoSpot.Data;
using TopAutoSpot.Models;
using TopAutoSpot.Models.Utilities;

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

        public IActionResult OnGet(string id)
        {
            if (id == null || _context.Auctions.Count() == 0)
            {
                return RedirectToPage("/NotFound");
            }

            Auction? auction = _context.Auctions
                .AsNoTracking()
                .FirstOrDefault(b => b.Id == id);

            User foundUser = _context.Users
                .AsNoTracking()
                .First(u => u.UserName == User.Identity.Name);

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
            User foundUser = _context.Users
                .AsNoTracking()
                .First(u => u.Id == Auction.AuctioneerId);

            return foundUser.FirstName + " " + foundUser.LastName;
        }

        public string GetImage()
        {
            byte[] data = _context.VehicleImages
                .AsNoTracking()
                .Where(img => img.VehicleId == Auction.VehicleId)
                .First()
                .ImageData;

            string imgDataURL = "data:image;base64," + Convert.ToBase64String(data);
            return imgDataURL;
        }
    }
}
