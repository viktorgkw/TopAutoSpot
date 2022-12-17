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

        public string GetImage(string truckId)
        {
            var data = _context.VehicleImages.First(i => i.VehicleId == truckId).ImageData;
            string imgDataURL = "data:image;base64," + Convert.ToBase64String(data);
            return imgDataURL;
        }

        public bool HasAnyImages(string truckId)
        {
            return _context.VehicleImages.Where(img => img.VehicleId == truckId).ToList().Count > 0;
        }
    }
}
