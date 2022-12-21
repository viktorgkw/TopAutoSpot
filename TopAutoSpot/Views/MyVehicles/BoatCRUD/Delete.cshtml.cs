using TopAutoSpot.Data;
using TopAutoSpot.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace TopAutoSpot.Views.MyVehicles.BoatCRUD
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
        public Boat Boat { get; set; } = default!;
        public List<VehicleImage> Images { get; set; }

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

            Images = _context.VehicleImages.Where(img => img.VehicleId == boat.Id).ToList();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null || _context.Boats == null)
            {
                return RedirectToPage("/Index");
            }
            var boat = await _context.Boats.FindAsync(id);

            if (boat != null)
            {
                RemoveVehicleImages(boat.Id);

                Boat = boat;
                _context.Boats.Remove(Boat);
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
