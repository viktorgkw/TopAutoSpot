using TopAutoSpot.Data;
using TopAutoSpot.Models;
using TopAutoSpot.Models.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace TopAutoSpot.Views.VehiclePreview
{
    public class BoatPreviewModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public BoatPreviewModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Boat Boat { get; set; } = default!;
        public List<VehicleImage> Images { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null || _context.Boats == null)
            {
                return RedirectToPage("/NotFound");
            }

            var boat = await _context.Boats.FirstOrDefaultAsync(b => b.Id == id);
            var foundUser = await _context.Users.FirstAsync(u => u.UserName == User.Identity.Name);

            if (boat == null)
            {
                return RedirectToPage("/NotFound");
            }
            else if (boat.Status != StatusTypes.Active.ToString() && boat.CreatedBy != foundUser.Id)
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

        public string GetOwnerNumber()
        {
            var foundUser = _context.Users
                .First(u => u.Id == Boat.CreatedBy);

            return foundUser.PhoneNumber;
        }

        public string GetOwnerFullName()
        {
            var foundUser = _context.Users
                .First(u => u.Id == Boat.CreatedBy);

            return foundUser.FirstName + " " + foundUser.LastName;
        }

        public string GetImage()
        {
            var data = _context.VehicleImages.Where(img => img.VehicleId == Boat.Id).First().ImageData;
            string imgDataURL = "data:image;base64," + Convert.ToBase64String(data);
            return imgDataURL;
        }

        public bool HasAnyImages()
        {
            return _context.VehicleImages.Where(img => img.VehicleId == Boat.Id).ToList().Count > 0;
        }

        public string GetImageSource(VehicleImage img)
        {
            string imgDataURL = "data:image;base64," + Convert.ToBase64String(img.ImageData);
            return imgDataURL;
        }
    }
}
