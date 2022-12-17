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

        public string GetImage(string motorcycleId)
        {
            var data = _context.VehicleImages.First(i => i.VehicleId == motorcycleId).ImageData;
            string imgDataURL = "data:image;base64," + Convert.ToBase64String(data);
            return imgDataURL;
        }

        public bool HasAnyImages(string motorcycleId)
        {
            return _context.VehicleImages.Where(img => img.VehicleId == motorcycleId).ToList().Count > 0;
        }
    }
}
