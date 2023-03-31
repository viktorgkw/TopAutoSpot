using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopAutoSpot.Data;
using TopAutoSpot.Data.Models;

namespace TopAutoSpot.Views.InterestedIn
{
    [Authorize]
    public class RemoveInterestModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public RemoveInterestModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet(string vehicleId)
        {
            if (vehicleId == null)
            {
                return RedirectToPage("/NotFound");
            }

            User currentUser = _context.Users
                .First(u => u.UserName == User.Identity!.Name);

            InterestedListing? foundInterestedListing = _context.InterestedInListings
                .FirstOrDefault(l => l.VehicleId == vehicleId);

            if (foundInterestedListing == null)
            {
                return RedirectToPage("/NotFound");
            }

            _context.InterestedInListings.Remove(foundInterestedListing);

            _context.SaveChanges();

            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}
