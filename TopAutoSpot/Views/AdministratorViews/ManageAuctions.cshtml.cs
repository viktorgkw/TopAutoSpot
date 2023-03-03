using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TopAutoSpot.Data;
using TopAutoSpot.Data.Models;

namespace TopAutoSpot.Views.AdministratorViews
{
    [Authorize]
    public class ManageAuctionsModel : PageModel
    {
        private ApplicationDbContext _context;

        public ManageAuctionsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public List<Auction> Auctions { get; set; }

        public async Task<IActionResult> OnGet()
        {
            if (User.IsInRole("Administrator"))
            {
                Auctions = _context.Auctions
                    .AsNoTracking()
                    .ToList();

                return Page();
            }

            return RedirectToPage("/NotFound");
        }

        public string GetAuctionImage(string auctionId)
        {
            string carId = _context.Auctions
                .AsNoTracking()
                .First(a => a.Id == auctionId)
                .VehicleId;

            byte[] data = _context.VehicleImages
                .AsNoTracking()
                .First(i => i.VehicleId == carId)
                .ImageData;

            string imgDataURL = "data:image;base64," + Convert.ToBase64String(data);
            return imgDataURL;
        }
    }
}
