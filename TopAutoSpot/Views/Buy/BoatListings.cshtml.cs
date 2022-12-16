using Microsoft.AspNetCore.Mvc.RazorPages;
using TopAutoSpot.Data.Entities;
using TopAutoSpot.Data;
using Microsoft.EntityFrameworkCore;

namespace TopAutoSpot.Views.Buy
{
    public class BoatListingsModel : PageModel
    {
        private ApplicationDbContext _context;
        public BoatListingsModel(ApplicationDbContext db)
        {
            _context = db;
        }

        public List<Boat> Boats { get; set; }

        public async Task OnGetAsync()
        {
            Boats = await _context.Boats.ToListAsync();
        }
    }
}
