using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopAutoSpot.Data;
using TopAutoSpot.Models;
using TopAutoSpot.Models.Utilities;

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

        public IActionResult OnGet()
        {
            if (User.IsInRole("Administrator"))
            {
                Auctions = _context.Auctions
                    .Where(a => a.Status == AuctionStatusTypes.WaitingApproval.ToString())
                    .ToList();

                return Page();
            }

            return RedirectToPage("/NotFound");
        }

        public string GetAuctionImage(string auctionId)
        {
            string carId = _context.Auctions
                .First(a => a.Id == auctionId)
                .VehicleId;

            byte[] data = _context.VehicleImages
                .First(i => i.VehicleId == carId)
                .ImageData;

            string imgDataURL = "data:image;base64," + Convert.ToBase64String(data);
            return imgDataURL;
        }
    }
}
