using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TopAutoSpot.Data;
using TopAutoSpot.Data.Entities;

namespace TopAutoSpot.Views.MyVehicles.BusCRUD
{
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Bus Bus { get; set; } = default!;
        public List<VehicleImage> Images { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null || _context.Buses == null)
            {
                return NotFound();
            }

            var bus = await _context.Buses.FirstOrDefaultAsync(b => b.Id == id);

            if (bus == null)
            {
                return NotFound();
            }
            else
            {
                Bus = bus;
            }

            Images = _context.VehicleImages.Where(img => img.VehicleId == bus.Id).ToList();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null || _context.Buses == null)
            {
                return NotFound();
            }
            var bus = await _context.Buses.FindAsync(id);

            if (bus != null)
            {
                RemoveVehicleImages(bus.Id);

                Bus = bus;
                _context.Buses.Remove(Bus);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/MyVehicles/Index");
        }

        private void RemoveVehicleImages(string vehicleId)
        {
            var images = _context.VehicleImages.Where(i => i.VehicleId == vehicleId).ToList();

            if (images.Count > 0)
            {
                foreach (var image in images)
                {
                    _context.VehicleImages.Remove(image);
                    _context.SaveChanges();
                }
            }
        }

        public string GetImageSource(VehicleImage img)
        {
            string imgDataURL = "data:image;base64," + Convert.ToBase64String(img.ImageData);
            return imgDataURL;
        }
    }
}
