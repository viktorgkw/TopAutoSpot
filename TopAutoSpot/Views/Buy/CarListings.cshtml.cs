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

        public string GetImage(string carId)
        {
            var data = _context.VehicleImages.First(i => i.VehicleId == carId).ImageData;
            string imgDataURL = "data:image;base64," + Convert.ToBase64String(data);
            return imgDataURL;
        }

        public bool HasAnyImages(string carId)
        {
            return _context.VehicleImages.Where(img => img.VehicleId == carId).ToList().Count > 0;
        }
    }
}
