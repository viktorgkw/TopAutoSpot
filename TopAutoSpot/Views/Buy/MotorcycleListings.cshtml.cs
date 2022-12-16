using Microsoft.AspNetCore.Mvc.RazorPages;
using TopAutoSpot.Data.Entities;
using TopAutoSpot.Data;
using Microsoft.EntityFrameworkCore;

namespace TopAutoSpot.Views.Buy
{
    public class MotorcycleListingsModel : PageModel
    {
        private ApplicationDbContext _context;
        public MotorcycleListingsModel(ApplicationDbContext db)
        {
            _context = db;
        }

        public List<Motorcycle> Motorcycles { get; set; }

        public async Task OnGetAsync()
        {
            Motorcycles = await _context.Motorcycles.ToListAsync();
        }
    }
}
