using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TopAutoSpot.Data;
using TopAutoSpot.Data.Entities;

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

        public async Task<IActionResult> OnGetAsync()
        {
            var currentUser = await _context.Users.FirstAsync(u => u.UserName == User.Identity.Name);

            InterestedListings = await _context.InterestedInListings
                .Where(l => l.UserId == currentUser.Id)
                .ToListAsync();

            return Page();
        }

        public async Task<string> GetCarTitle(string vehId)
        {
            return _context.Cars.First(c => c.Id == vehId).Title;
        }

        public async Task<string> GetMotoTitle(string vehId)
        {
            return _context.Motorcycles.First(c => c.Id == vehId).Title;
        }

        public async Task<string> GetTruckTitle(string vehId)
        {
            return _context.Trucks.First(c => c.Id == vehId).Title;
        }

        public async Task<string> GetTrailerTitle(string vehId)
        {
            return _context.Trailers.First(c => c.Id == vehId).Title;
        }

        public async Task<string> GetBusTitle(string vehId)
        {
            return _context.Buses.First(c => c.Id == vehId).Title;
        }

        public async Task<string> GetBoatTitle(string vehId)
        {
            return _context.Boats.First(c => c.Id == vehId).Title;
        }
    }
}
