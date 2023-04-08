namespace TopAutoSpot.Views.AuctionViews
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    using TopAutoSpot.Data;
    using TopAutoSpot.Data.Models;

    /// <summary>
    /// Represents the Join page model for joining an auction.
    /// </summary>
    [Authorize]
    public class JoinModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public JoinModel(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Handles the GET request to the Join page.
        /// </summary>
        /// <param name="id">The ID of the auction to join.</param>
        /// <returns>The appropriate IActionResult.</returns>
        public IActionResult OnGet(string id)
        {
            Auction? foundAuction = _context.Auctions.FirstOrDefault(a => a.Id == id);

            if (foundAuction == null)
            {
                return RedirectToPage("/NotFound");
            }

            if (foundAuction.Status != "Active")
            {
                return RedirectToPage("/AuctionViews/Index");
            }

            User? foundUser = _context.Users.FirstOrDefault(u => u.UserName == User.Identity!.Name);

            if (foundUser == null)
            {
                return RedirectToPage("/UnknownError");
            }

            if (foundAuction.Bidders!.Any(b => b.UserName == foundUser.UserName))
            {
                return RedirectToPage("/AuctionViews/Index");
            }

            foundAuction.Bidders!.Add(foundUser);
            _context.SaveChanges();

            return RedirectToPage("/AuctionViews/Index");
        }
    }
}
