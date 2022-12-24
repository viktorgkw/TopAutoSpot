using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopAutoSpot.Data;

namespace TopAutoSpot.Views.AuctionViews
{
    [Authorize]
    public class LeaveModel : PageModel
    {
        private ApplicationDbContext _context;
        public LeaveModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet(string id)
        {
            Models.Auction? foundAuction = _context.Auctions.FirstOrDefault(a => a.Id == id);

            if (foundAuction == null)
            {
                return RedirectToPage("/NotFound");
            }

            if (foundAuction.Status != "Active")
            {
                return RedirectToPage("/AuctionViews/Index");
            }

            Models.User? foundUser = _context.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);

            if (foundUser == null)
            {
                return RedirectToPage("/UnknownError");
            }

            if (!foundAuction.Bidders.Any(b => b.UserName == foundUser.UserName))
            {
                return RedirectToPage("/AuctionViews/Index");
            }

            foundAuction.Bidders.Remove(foundUser);
            _context.SaveChanges();

            return RedirectToPage("/AuctionViews/JoinedAuctions");
        }
    }
}
