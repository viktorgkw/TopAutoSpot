using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopAutoSpot.Data;
using TopAutoSpot.Models;

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

        public IActionResult OnGet(string id)
        {
            if (id == null || _context.Trucks == null)
            {
                return RedirectToPage("/NotFound");
            }

            Truck? truck = _context.Trucks.FirstOrDefault(m => m.Id == id);
            User foundUser = _context.Users.First(u => u.UserName == User.Identity.Name);

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

            Images = _context.VehicleImages
                .Where(img => img.VehicleId == truck.Id)
                .ToList();

            return Page();
        }

        public IActionResult OnPost(string id)
        {
            if (id == null || _context.Trucks == null)
            {
                return RedirectToPage("/Index");
            }
            Truck? truck = _context.Trucks.Find(id);

            if (truck != null)
            {
                RemoveVehicleImages(truck.Id);

                Truck = truck;
                _context.Trucks.Remove(Truck);
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
