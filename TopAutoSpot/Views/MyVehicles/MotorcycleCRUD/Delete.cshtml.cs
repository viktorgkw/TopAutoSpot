using TopAutoSpot.Data;
using TopAutoSpot.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace TopAutoSpot.Views.MyVehicles.MotorcycleCRUD
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Motorcycle Motorcycle { get; set; } = default!;
        public List<VehicleImage> Images { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null || _context.Motorcycles == null)
            {
                return RedirectToPage("/NotFound");
            }

            var motorcycle = await _context.Motorcycles.FirstOrDefaultAsync(m => m.Id == id);
            var foundUser = await _context.Users.FirstAsync(u => u.UserName == User.Identity.Name);

            if (motorcycle == null)
            {
                return RedirectToPage("/NotFound");
            }
            else if (motorcycle.CreatedBy != foundUser.Id)
            {
                return RedirectToPage("/MyVehicles/Index");
            }
            else
            {
                Motorcycle = motorcycle;
            }

            Images = _context.VehicleImages.Where(img => img.VehicleId == motorcycle.Id).ToList();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null || _context.Motorcycles == null)
            {
                return RedirectToPage("/Index");
            }
            var motorcycle = await _context.Motorcycles.FindAsync(id);

            if (motorcycle != null)
            {
                RemoveVehicleImages(motorcycle.Id);

                Motorcycle = motorcycle;
                _context.Motorcycles.Remove(Motorcycle);
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
