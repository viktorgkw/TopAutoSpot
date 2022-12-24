using TopAutoSpot.Data;
using TopAutoSpot.Models;
using TopAutoSpot.Views.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;

namespace TopAutoSpot.Views.AuctionViews
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var currentUserId = await UserServices.GetCurrentUser(_context, User.Identity.Name);
            CurrentUserVehicles = await UserServices.GetUserVehicles(_context, currentUserId);

            if (CurrentUserVehicles.Count == 0)
            {
                return RedirectToPage("/AuctionViews/NoVehiclesToAuction");
            }

            return Page();
        }

        [BindProperty]
        public Auction Auction { get; set; } = default!;
        public List<string> CurrentUserVehicles { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            Auction.AuctioneerId = await UserServices.GetCurrentUser(_context, User.Identity.Name);
            Auction.VehicleId = await UserServices.GetVehicleIdByTitle(_context, Auction.VehicleId);

            if (Auction.VehicleId == "")
            {
                return RedirectToPage("/NotFound");
            }

            if (!ModelState.IsValid || _context.Auctions == null || Auction == null)
            {
                return RedirectToPage("/NotFound");
            }

            Auction.Bidders = new List<User>();

            await _context.Auctions.AddAsync(Auction);
            await _context.SaveChangesAsync();

            return RedirectToPage("/AuctionViews/Index");
        }
    }
}
