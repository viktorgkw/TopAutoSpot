using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TopAutoSpot.Data;
using TopAutoSpot.Models;

namespace TopAutoSpot.Views.MyVehicles.BoatCRUD
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
        public Boat Boat { get; set; } = default!;
        public VehicleImage VehicleImage { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null || _context.Boats == null)
            {
                return RedirectToPage("/NotFound");
            }

            var boat = await _context.Boats.FirstOrDefaultAsync(m => m.Id == id);
            var foundUser = await _context.Users.FirstAsync(u => u.UserName == User.Identity.Name);

            if (boat == null)
            {
                return RedirectToPage("/NotFound");
            }
            else if (boat.CreatedBy != foundUser.Id)
            {
                return RedirectToPage("/MyVehicles/Index");
            }
            else
            {
                Boat = boat;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(List<IFormFile> Images)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Boat).State = EntityState.Modified;

            try
            {
                await AddImagesToVehicle(Images, Boat.Id);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BoatExists(Boat.Id))
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

        private bool BoatExists(string id)
        {
            return (_context.Boats?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private async Task AddImagesToVehicle(List<IFormFile> images, string vehicleId)
        {
            images = FilterImages(images);

            if (images.Count > 0 || images != null)
            {
                await RemoveExistingVehicleImages(vehicleId);

                foreach (IFormFile image in images)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        await image.CopyToAsync(ms);

                        var vehicleImage = new VehicleImage()
                        {

                            Id = Guid.NewGuid().ToString(),
                            ImageName = image.FileName,
                            ImageData = ms.ToArray(),
                            VehicleId = vehicleId,
                        };

                        await _context.VehicleImages.AddAsync(vehicleImage);
                        await _context.SaveChangesAsync();
                    }
                }
            }
        }

        private async Task RemoveExistingVehicleImages(string vehicleId)
        {
            var foundImages = await _context.VehicleImages
                .Where(i => i.VehicleId == vehicleId)
                .ToListAsync();

            foreach (var img in foundImages)
            {
                _context.Remove(img);
                await _context.SaveChangesAsync();
            }
        }

        private List<IFormFile> FilterImages(List<IFormFile> images)
        {
            images = images
                .Where(i =>
                    i.FileName.EndsWith(".png") ||
                    i.FileName.EndsWith(".jpeg") ||
                    i.FileName.EndsWith(".jpg"))
                .ToList();

            return images;
        }
    }
}
