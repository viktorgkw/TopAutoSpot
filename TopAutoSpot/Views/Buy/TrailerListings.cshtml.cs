using Microsoft.AspNetCore.Mvc.RazorPages;
using TopAutoSpot.Data.Entities;
using TopAutoSpot.Data;
using Microsoft.EntityFrameworkCore;

namespace TopAutoSpot.Views.Buy
{
    public class TrailerListingsModel : PageModel
    {
        private ApplicationDbContext _context;
        public TrailerListingsModel(ApplicationDbContext db)
        {
            _context = db;
        }

        public List<Trailer> Trailers { get; set; }

        public async Task OnGetAsync()
        {
            Trailers = await _context.Trailers.ToListAsync();
        }
    }
}
