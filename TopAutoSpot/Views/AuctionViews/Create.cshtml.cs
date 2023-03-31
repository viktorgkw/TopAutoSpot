using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopAutoSpot.Data;
using TopAutoSpot.Data.Models;
using TopAutoSpot.Views.Utilities;

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

        public IActionResult OnGet()
        {
            string currentUserId = UserServices.GetCurrentUser(_context, User.Identity!.Name!);
            CurrentUserVehicles = UserServices.GetUserVehicles(_context, currentUserId);

            if (CurrentUserVehicles.Count == 0)
            {
                return RedirectToPage("/AuctionViews/NoVehiclesToAuction");
            }

            return Page();
        }

        [BindProperty]
        public Auction Auction { get; set; } = default!;
        public List<string> CurrentUserVehicles { get; set; } = default!;

        public IActionResult OnPost()
        {
            if (Auction.StartDay.CompareTo(DateTime.Now) <= 0)
            {
                return RedirectToPage("/AuctionViews/Index");
            }

            Auction.AuctioneerId = UserServices.GetCurrentUser(_context, User.Identity!.Name!);
            Auction.VehicleId = UserServices.GetVehicleIdByTitle(_context, Auction.VehicleId);

            if (Auction.VehicleId == "")
            {
                return RedirectToPage("/NotFound");
            }

            if (!ModelState.IsValid || !_context.Auctions.Any() || Auction == null)
            {
                return RedirectToPage("/NotFound");
            }

            Auction.Bidders = new List<User>();

            _context.Auctions.Add(Auction);
            _context.SaveChanges();

            return RedirectToPage("/AuctionViews/Index");
        }
    }
}
