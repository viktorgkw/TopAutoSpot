using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TopAutoSpot.Data;
using TopAutoSpot.Models;

namespace TopAutoSpot.Views.InterestedIn
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private ApplicationDbContext _context;
        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<InterestedListing> InterestedListings { get; set; }

        public IActionResult OnGet()
        {
            User currentUser = _context.Users
                .AsNoTracking()
                .First(u => u.UserName == User.Identity.Name);

            InterestedListings = _context.InterestedInListings
                .AsNoTracking()
                .Where(l => l.UserId == currentUser.Id)
                .ToList();

            return Page();
        }

        public string GetCarTitle(string vehId)
        {
            return _context.Cars
                .AsNoTracking()
                .First(c => c.Id == vehId).Title;
        }

        public string GetMotoTitle(string vehId)
        {
            return _context.Motorcycles
                .AsNoTracking()
                .First(c => c.Id == vehId).Title;
        }

        public string GetTruckTitle(string vehId)
        {
            return _context.Trucks
                .AsNoTracking()
                .First(c => c.Id == vehId).Title;
        }

        public string GetTrailerTitle(string vehId)
        {
            return _context.Trailers
                .AsNoTracking()
                .First(c => c.Id == vehId).Title;
        }

        public string GetBusTitle(string vehId)
        {
            return _context.Buses
                .AsNoTracking()
                .First(c => c.Id == vehId).Title;
        }

        public string GetBoatTitle(string vehId)
        {
            return _context.Boats
                .AsNoTracking()
                .First(c => c.Id == vehId).Title;
        }
    }
}
