using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopAutoSpot.Data;
using TopAutoSpot.Data.Models;

namespace TopAutoSpot.Views.AuctionViews
{
    [Authorize]
    public class JoinModel : PageModel
    {
        private ApplicationDbContext _context;
        public JoinModel(ApplicationDbContext context)
        {
            _context = context;
        }

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

            User? foundUser = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);

            if (foundUser == null)
            {
                return RedirectToPage("/UnknownError");
            }

            if (foundAuction.Bidders.Any(b => b.UserName == foundUser.UserName))
            {
                return RedirectToPage("/AuctionViews/Index");
            }

            foundAuction.Bidders.Add(foundUser);
            _context.SaveChanges();

            return RedirectToPage("/AuctionViews/Index");
        }
    }
}
