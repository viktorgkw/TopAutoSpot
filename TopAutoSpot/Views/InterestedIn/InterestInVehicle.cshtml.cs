using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using TopAutoSpot.Data;
using TopAutoSpot.Data.Models;

namespace TopAutoSpot.Views.InterestedIn
{
    [Authorize]
    public class InterestInVehicleModel : PageModel
    {
        private ApplicationDbContext _context;

        public InterestInVehicleModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(string vehicleId, string vehicleCategory)
        {
            if (vehicleCategory == null || vehicleId == null)
            {
                return RedirectToPage("/NotFound");
            }

            User currentUser = await _context.Users
                .FirstAsync(u => u.UserName == User.Identity.Name);

            await _context.InterestedInListings.AddAsync(new InterestedListing()
            {
                Id = Guid.NewGuid().ToString(),
                UserId = currentUser.Id,
                VehicleId = vehicleId,
                VehicleCategory = vehicleCategory
            });

            await _context.SaveChangesAsync();

            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}
