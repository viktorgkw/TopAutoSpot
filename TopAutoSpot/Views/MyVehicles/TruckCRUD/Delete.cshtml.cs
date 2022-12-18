using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TopAutoSpot.Data;
using TopAutoSpot.Data.Entities;

namespace TopAutoSpot.Views.MyVehicles.TruckCRUD
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
        public Truck Truck { get; set; } = default!;
        public List<VehicleImage> Images { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null || _context.Trucks == null)
            {
                return RedirectToPage("/NotFound");
            }

            var truck = await _context.Trucks.FirstOrDefaultAsync(m => m.Id == id);
            var foundUser = await _context.Users.FirstAsync(u => u.UserName == User.Identity.Name);

            if (truck == null)
            {
                return RedirectToPage("/NotFound");
            }
            else if (truck.CreatedBy != foundUser.Id)
            {
                return RedirectToPage("/MyVehicles/Index");
            }
            else
            {
                Truck = truck;
            }

            Images = _context.VehicleImages.Where(img => img.VehicleId == truck.Id).ToList();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null || _context.Trucks == null)
            {
                return RedirectToPage("/Index");
            }
            var truck = await _context.Trucks.FindAsync(id);

            if (truck != null)
            {
                RemoveVehicleImages(truck.Id);

                Truck = truck;
                _context.Trucks.Remove(Truck);
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
