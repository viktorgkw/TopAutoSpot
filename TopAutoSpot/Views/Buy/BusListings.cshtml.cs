using Microsoft.AspNetCore.Mvc.RazorPages;
using TopAutoSpot.Data.Entities;
using TopAutoSpot.Data;
using Microsoft.EntityFrameworkCore;

namespace TopAutoSpot.Views.Buy
{
    public class BusListingsModel : PageModel
    {
        private ApplicationDbContext _context;
        public BusListingsModel(ApplicationDbContext db)
        {
            _context = db;
        }

        public List<Bus> Buses { get; set; }

        public async Task OnGetAsync()
        {
            Buses = await _context.Buses.ToListAsync();
        }
    }
}
