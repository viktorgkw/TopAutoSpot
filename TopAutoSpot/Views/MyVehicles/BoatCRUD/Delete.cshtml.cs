using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopAutoSpot.Data;
using TopAutoSpot.Data.Models;

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
        public List<VehicleImage> Images { get; set; } = null!;

        public IActionResult OnGet(string id)
        {
            if (id == null || _context.Boats.Count() == 0)
            {
                return RedirectToPage("/NotFound");
            }

            Boat? boat = _context.Boats.FirstOrDefault(m => m.Id == id);
            User foundUser = _context.Users.First(u => u.UserName == User.Identity!.Name);

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

            Images = _context.VehicleImages
                .Where(img => img.VehicleId == boat.Id)
                .ToList();

            return Page();
        }

        public IActionResult OnPost(string id)
        {
            if (id == null || _context.Boats.Count() == 0)
            {
                return RedirectToPage("/Index");
            }
            Boat? boat = _context.Boats.Find(id);

            if (boat != null)
            {
                RemoveVehicleImages(boat.Id);

                Boat = boat;
                _context.Boats.Remove(Boat);
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
