using TopAutoSpot.Data;
using TopAutoSpot.Models;
using TopAutoSpot.Models.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace TopAutoSpot.Views.AdministratorViews
{
    [Authorize]
    public class AuctionsApprovalModel : PageModel
    {
        private ApplicationDbContext _context;

        public AuctionsApprovalModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Auction> Auctions { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (User.IsInRole("Administrator"))
            {
                Auctions = await _context.Auctions
                    .Where(a => a.Status == AuctionStatusTypes.WaitingApproval.ToString())
                    .ToListAsync();

                return Page();
            }

            return RedirectToPage("/NotFound");
        }

        public string GetAuctionImage(string auctionId)
        {
            var carId = _context.Auctions.First(a => a.Id == auctionId).VehicleId;
            var data = _context.VehicleImages.First(i => i.VehicleId == carId).ImageData;
            string imgDataURL = "data:image;base64," + Convert.ToBase64String(data);
            return imgDataURL;
        }
    }
}
