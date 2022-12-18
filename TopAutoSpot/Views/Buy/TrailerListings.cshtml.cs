using Microsoft.AspNetCore.Mvc.RazorPages;
using TopAutoSpot.Data.Entities;
using TopAutoSpot.Data;
using Microsoft.EntityFrameworkCore;
using TopAutoSpot.Data.Entities.Utilities;

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
            Trailers = await _context.Trailers
                .Where(t => t.Status == StatusTypes.Active.ToString())
                .ToListAsync();
        }

        public string GetImage(string trailerId)
        {
            var data = _context.VehicleImages.First(i => i.VehicleId == trailerId).ImageData;
            string imgDataURL = "data:image;base64," + Convert.ToBase64String(data);
            return imgDataURL;
        }

        public bool HasAnyImages(string trailerId)
        {
            return _context.VehicleImages.Where(img => img.VehicleId == trailerId).ToList().Count > 0;
        }
    }
}
