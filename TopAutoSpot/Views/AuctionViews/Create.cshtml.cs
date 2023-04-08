namespace TopAutoSpot.Views.AuctionViews
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    using TopAutoSpot.Data;
    using TopAutoSpot.Data.Models;
    using TopAutoSpot.Services.Utilities;

    /// <summary>
    /// This C# code defines a PageModel class called CreateModel that contains methods for creating a new auction.
    /// The class is decorated with the [Authorize] attribute, which means that only authorized users can access its methods.
    /// The class constructor takes an instance of ApplicationDbContext, which is used to interact with the database
    /// </summary>
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// HTTP GET method that fetches the current user's vehicles from the database and returns the Create page if the user has at least one vehicle, otherwise it redirects to the NoVehiclesToAuction page.
        /// </summary>
        /// <returns>IActionResult</returns>
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

        /// <summary>
        /// HTTP POST method that creates a new auction by taking input from the Create page's form, validating the input, and then adding the auction to the database. It also sets the auctioneer and vehicle ID.
        /// </summary>
        /// <returns>IActionResult</returns>
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
