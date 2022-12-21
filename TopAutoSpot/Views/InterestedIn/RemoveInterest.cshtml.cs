using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopAutoSpot.Data;
using TopAutoSpot.Data.Entities;

namespace TopAutoSpot.Views.InterestedIn
{
    [Authorize]
    public class RemoveInterestModel : PageModel
    {
        private ApplicationDbContext _context;

        public RemoveInterestModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(string vehicleId)
        {
            if (vehicleId == null)
            {
                return RedirectToPage("/NotFound");
            }

            var currentUser = _context.Users
                .First(u => u.UserName == User.Identity.Name);

            var foundInterestedListing = _context.InterestedInListings
                .FirstOrDefault(l => l.VehicleId == vehicleId);

            if (foundInterestedListing == null)
            {
                return RedirectToPage("/NotFound");
            }

            _context.InterestedInListings.Remove(foundInterestedListing);

            await _context.SaveChangesAsync();

            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}
