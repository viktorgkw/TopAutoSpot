using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TopAutoSpot.Data;
using TopAutoSpot.Data.Models;

namespace TopAutoSpot.Views.MyVehicles.BusCRUD
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
        public Bus Bus { get; set; } = default!;
        public VehicleImage VehicleImage { get; set; } = default!;

        public IActionResult OnGet(string id)
        {
            if (id == null || _context.Buses.Count() == 0)
            {
                return RedirectToPage("/NotFound");
            }

            Bus? bus = _context.Buses
                .FirstOrDefault(b => b.Id == id);
            User foundUser = _context.Users
                .First(u => u.UserName == User.Identity!.Name);

            if (bus == null)
            {
                return RedirectToPage("/NotFound");
            }
            else if (bus.CreatedBy != foundUser.Id)
            {
                return RedirectToPage("/MyVehicles/Index");
            }
            else
            {
                Bus = bus;
            }

            return Page();
        }

        public IActionResult OnPost(List<IFormFile> Images)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Bus).State = EntityState.Modified;

            try
            {
                AddImagesToVehicle(Images, Bus.Id);
                _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BusExists(Bus.Id))
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

        private bool BusExists(string id)
        {
            return (_context.Buses?.Any(e => e.Id == id)).GetValueOrDefault();
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
