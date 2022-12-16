using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TopAutoSpot.Data;
using TopAutoSpot.Data.Entities;

namespace TopAutoSpot.Views.Buy
{
    public class CarListingsModel : PageModel
    {
        private ApplicationDbContext _context;
        public CarListingsModel(ApplicationDbContext db)
        {
            _context = db;
        }

        public List<Car> Cars { get; set; }

        public async Task OnGetAsync()
        {
            Cars = await _context.Cars.ToListAsync();
        }
    }
}
