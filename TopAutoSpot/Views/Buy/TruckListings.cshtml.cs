using Microsoft.AspNetCore.Mvc.RazorPages;
using TopAutoSpot.Data.Entities;
using TopAutoSpot.Data;
using Microsoft.EntityFrameworkCore;

namespace TopAutoSpot.Views.Buy
{
    public class TruckListingsModel : PageModel
    {
        private ApplicationDbContext _context;
        public TruckListingsModel(ApplicationDbContext db)
        {
            _context = db;
        }

        public List<Truck> Trucks { get; set; }

        public async Task OnGetAsync()
        {
            Trucks = await _context.Trucks.ToListAsync();
        }
    }
}
