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

        public string GetImage(string boatId)
        {
            var data = _context.VehicleImages.First(i => i.VehicleId == boatId).ImageData;
            string imgDataURL = "data:image;base64," + Convert.ToBase64String(data);
            return imgDataURL;
        }

        public bool HasAnyImages(string boatId)
        {
            return _context.VehicleImages.Where(img => img.VehicleId == boatId).ToList().Count > 0;
        }
    }
}
