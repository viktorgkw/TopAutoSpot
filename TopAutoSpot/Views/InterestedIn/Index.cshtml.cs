namespace TopAutoSpot.Views.InterestedIn
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;

    using TopAutoSpot.Data;
    using TopAutoSpot.Data.Models;

    /// <summary>
    /// This class represents the Index page model for a user's interested listings.
    /// It inherits from PageModel and is decorated with the Authorize attribute.
    /// </summary>
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Constructor for the IndexModel that takes an ApplicationDbContext object.
        /// </summary>
        /// <param name="context">The ApplicationDbContext object to be injected.</param>
        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Property for a list of InterestedListing objects.
        /// </summary>
        public List<InterestedListing> InterestedListings { get; set; } = null!;

        /// <summary>
        /// Handles HTTP GET requests for the Index page.
        /// </summary>
        /// <returns>An IActionResult representing the Index page.</returns>
        public IActionResult OnGet()
        {
            User currentUser = _context.Users
                .AsNoTracking()
                .First(u => u.UserName == User.Identity!.Name);

            InterestedListings = _context.InterestedInListings
                .AsNoTracking()
                .Where(l => l.UserId == currentUser.Id)
                .ToList();

            return Page();
        }

        /// <summary>
        /// Returns the title of a car with a given ID.
        /// </summary>
        /// <param name="vehId">The ID of the car.</param>
        /// <returns>A string representing the title of the car.</returns>
        public string GetCarTitle(string vehId)
        {
            return _context.Cars
                .AsNoTracking()
                .First(c => c.Id == vehId).Title;
        }

        /// <summary>
        /// Returns the title of a motorcycle with a given ID.
        /// </summary>
        /// <param name="vehId">The ID of the motorcycle.</param>
        /// <returns>A string representing the title of the motorcycle.</returns>
        public string GetMotoTitle(string vehId)
        {
            return _context.Motorcycles
                .AsNoTracking()
                .First(c => c.Id == vehId).Title;
        }

        /// <summary>
        /// Returns the title of a truck with a given ID.
        /// </summary>
        /// <param name="vehId">The ID of the truck.</param>
        /// <returns>A string representing the title of the truck.</returns>
        public string GetTruckTitle(string vehId)
        {
            return _context.Trucks
                .AsNoTracking()
                .First(c => c.Id == vehId).Title;
        }

        /// <summary>
        /// Returns the title of a trailer with a given ID.
        /// </summary>
        /// <param name="vehId">The ID of the trailer.</param>
        /// <returns>A string representing the title of the trailer.</returns>
        public string GetTrailerTitle(string vehId)
        {
            return _context.Trailers
                .AsNoTracking()
                .First(c => c.Id == vehId).Title;
        }

        /// <summary>
        /// Returns the title of a bus with a given ID.
        /// </summary>
        /// <param name="vehId">The ID of the bus.</param>
        /// <returns>A string representing the title of the bus.</returns>
        public string GetBusTitle(string vehId)
        {
            return _context.Buses
                .AsNoTracking()
                .First(c => c.Id == vehId).Title;
        }

        /// <summary>
        /// Returns the title of a boat with a given ID.
        /// </summary>
        /// <param name="vehId">The ID of the boat.</param>
        /// <returns>A string representing the title of the boat.</returns>
        public string GetBoatTitle(string vehId)
        {
            return _context.Boats
                .AsNoTracking()
                .First(c => c.Id == vehId).Title;
        }
    }
}
