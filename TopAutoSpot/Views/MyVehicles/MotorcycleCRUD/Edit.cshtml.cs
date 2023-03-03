using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TopAutoSpot.Data;
using TopAutoSpot.Data.Models;

namespace TopAutoSpot.Views.MyVehicles.MotorcycleCRUD
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Motorcycle Motorcycle { get; set; } = default!;
        public VehicleImage VehicleImage { get; set; } = default!;

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

            return Page();
        }

        public IActionResult OnPost(List<IFormFile> Images)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Motorcycle).State = EntityState.Modified;

            try
            {
                AddImagesToVehicle(Images, Motorcycle.Id);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MotorcycleExists(Motorcycle.Id))
                {
                    return RedirectToPage("/Index");
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("/MyVehicles/Index");
        }

        private bool MotorcycleExists(string id)
        {
            return (_context.Motorcycles?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private void AddImagesToVehicle(List<IFormFile> images, string vehicleId)
        {
            images = FilterImages(images);

            if (images.Count > 0)
            {
                RemoveExistingVehicleImages(vehicleId);

                foreach (IFormFile image in images)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        image.CopyTo(ms);

                        VehicleImage vehicleImage = new VehicleImage()
                        {

                            Id = Guid.NewGuid().ToString(),
                            ImageName = image.FileName,
                            ImageData = ms.ToArray(),
                            VehicleId = vehicleId,
                        };

                        _context.VehicleImages.Add(vehicleImage);
                        _context.SaveChanges();
                    }
                }
            }
        }

        private void RemoveExistingVehicleImages(string vehicleId)
        {
            List<VehicleImage> foundImages = _context.VehicleImages
                .Where(i => i.VehicleId == vehicleId)
                .ToList();

            foreach (VehicleImage? img in foundImages)
            {
                _context.Remove(img);
                _context.SaveChanges();
            }
        }

        private List<IFormFile> FilterImages(List<IFormFile> images)
        {
            images = images
                .Where(i =>
                    i.FileName.ToLower().EndsWith(".png") ||
                    i.FileName.ToLower().EndsWith(".jpeg") ||
                    i.FileName.ToLower().EndsWith(".jpg"))
                .ToList();

            return images;
        }
    }
}
