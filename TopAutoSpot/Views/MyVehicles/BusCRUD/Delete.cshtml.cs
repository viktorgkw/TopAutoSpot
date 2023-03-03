using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopAutoSpot.Data;
using TopAutoSpot.Models;

namespace TopAutoSpot.Views.MyVehicles.BusCRUD
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
        public Bus Bus { get; set; } = default!;
        public List<VehicleImage> Images { get; set; }

        public IActionResult OnGet(string id)
        {
            if (id == null || _context.Buses.Count() == 0)
            {
                return RedirectToPage("/NotFound");
            }

            Bus? bus = _context.Buses.FirstOrDefault(b => b.Id == id);
            User foundUser = _context.Users.First(u => u.UserName == User.Identity.Name);

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

            Images = _context.VehicleImages
                .Where(img => img.VehicleId == bus.Id)
                .ToList();

            return Page();
        }

        public IActionResult OnPost(string id)
        {
            if (id == null || _context.Buses.Count() == 0)
            {
                return RedirectToPage("/Index");
            }
            Bus? bus = _context.Buses.Find(id);

            if (bus != null)
            {
                RemoveVehicleImages(bus.Id);

                Bus = bus;
                _context.Buses.Remove(Bus);
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
