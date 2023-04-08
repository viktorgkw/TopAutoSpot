namespace TopAutoSpot.Views.InterestedIn
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;

    using TopAutoSpot.Data;
    using TopAutoSpot.Data.Models;

    /// <summary>
    /// PageModel class for creating a new interest in a vehicle listing.
    /// Requires user authentication.
    /// </summary>
    [Authorize]
    public class InterestInVehicleModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public InterestInVehicleModel(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Method for handling GET requests to add a new interest in a vehicle listing.
        /// Adds a new InterestedListing object to the database for the current user and specified vehicle.
        /// </summary>
        /// <param name="vehicleId">The ID of the vehicle being added to the user's interests.</param>
        /// <param name="vehicleCategory">The category of the vehicle being added to the user's interests.</param>
        /// <returns>Redirects to the previous page.</returns>
        public async Task<IActionResult> OnGetAsync(string vehicleId, string vehicleCategory)
        {
            if (vehicleCategory == null || vehicleId == null)
            {
                return RedirectToPage("/NotFound");
            }

            User currentUser = await _context.Users
                .FirstAsync(u => u.UserName == User.Identity!.Name);

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
