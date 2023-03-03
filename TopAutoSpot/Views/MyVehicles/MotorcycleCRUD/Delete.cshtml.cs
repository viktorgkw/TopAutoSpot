using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopAutoSpot.Data;
using TopAutoSpot.Data.Models;

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

        public IActionResult OnGet(string id)
        {
            if (id == null || _context.Motorcycles.Count() == 0)
            {
                return RedirectToPage("/NotFound");
            }

            Motorcycle? motorcycle = _context.Motorcycles.FirstOrDefault(m => m.Id == id);
            User foundUser = _context.Users.First(u => u.UserName == User.Identity.Name);

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

            Images = _context.VehicleImages
                .Where(img => img.VehicleId == motorcycle.Id)
                .ToList();

            return Page();
        }

        public async Task<IActionResult> OnPost(string id)
        {
            if (id == null || _context.Motorcycles.Count() == 0)
            {
                return RedirectToPage("/Index");
            }
            Motorcycle? motorcycle = _context.Motorcycles.Find(id);

            if (motorcycle != null)
            {
                RemoveVehicleImages(motorcycle.Id);

                Motorcycle = motorcycle;
                _context.Motorcycles.Remove(Motorcycle);
                _context.SaveChanges();
            }

            return RedirectToPage("/MyVehicles/Index");
        }

        private void RemoveVehicleImages(string vehicleId)
        {
            List<VehicleImage> images = _context.VehicleImages
                .Where(i => i.VehicleId == vehicleId)
                .ToList();

            if (images.Count > 0)
            {
                foreach (VehicleImage? image in images)
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
