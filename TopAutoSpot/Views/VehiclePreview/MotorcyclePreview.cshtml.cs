using Microsoft.AspNetCore.Mvc.RazorPages;
using TopAutoSpot.Data.Entities;
using TopAutoSpot.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace TopAutoSpot.Views.VehiclePreview
{
    public class MotorcyclePreviewModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public MotorcyclePreviewModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Motorcycle Motorcycle { get; set; } = default!;
        public List<VehicleImage> Images { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null || _context.Motorcycles == null)
            {
                return NotFound();
            }

            var moto = await _context.Motorcycles.FirstOrDefaultAsync(m => m.Id == id);
            if (moto == null)
            {
                return NotFound();
            }
            else
            {
                Motorcycle = moto;
            }

            Images = _context.VehicleImages.Where(img => img.VehicleId == moto.Id).ToList();
            return Page();
        }

        public string GetOwnerNumber()
        {
            var foundUser = _context.Users
                .First(u => u.Id == Motorcycle.CreatedBy);

            return foundUser.PhoneNumber;
        }

        public string GetOwnerFullName()
        {
            var foundUser = _context.Users
                .First(u => u.Id == Motorcycle.CreatedBy);

            return foundUser.FirstName + " " + foundUser.LastName;
        }

        public string GetImage()
        {
            var data = _context.VehicleImages.Where(img => img.VehicleId == Motorcycle.Id).First().ImageData;
            string imgDataURL = "data:image;base64," + Convert.ToBase64String(data);
            return imgDataURL;
        }

        public bool HasAnyImages()
        {
            return _context.VehicleImages.Where(img => img.VehicleId == Motorcycle.Id).ToList().Count > 0;
        }

        public string GetImageSource(VehicleImage img)
        {
            string imgDataURL = "data:image;base64," + Convert.ToBase64String(img.ImageData);
            return imgDataURL;
        }
    }
}
