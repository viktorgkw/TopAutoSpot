using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopAutoSpot.Data;
using TopAutoSpot.Models;
using TopAutoSpot.Models.Utilities;

namespace TopAutoSpot.Views.VehiclePreview
{
    public class BusPreviewModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public BusPreviewModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Bus Bus { get; set; } = default!;
        public List<VehicleImage> Images { get; set; }

        public IActionResult OnGet(string id)
        {
            if (id == null || _context.Buses == null)
            {
                return RedirectToPage("/NotFound");
            }

            Bus? bus = _context.Buses.FirstOrDefault(b => b.Id == id);
            User foundUser = _context.Users.First(u => u.UserName == User.Identity.Name);

            if (bus == null)
            {
                return RedirectToPage("/NotFound");
            }
            else if (bus.Status != ListingStatusTypes.Active.ToString() && bus.CreatedBy != foundUser.Id)
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

        public string GetOwnerNumber()
        {
            User foundUser = _context.Users
                .First(u => u.Id == Bus.CreatedBy);

            return foundUser.PhoneNumber;
        }

        public string GetOwnerFullName()
        {
            User foundUser = _context.Users
                .First(u => u.Id == Bus.CreatedBy);

            return foundUser.FirstName + " " + foundUser.LastName;
        }

        public string GetImage()
        {
            byte[] data = _context.VehicleImages
                .Where(img => img.VehicleId == Bus.Id)
                .First()
                .ImageData;

            string imgDataURL = "data:image;base64," + Convert.ToBase64String(data);
            return imgDataURL;
        }

        public bool HasAnyImages()
        {
            return _context.VehicleImages
                .Where(img => img.VehicleId == Bus.Id)
                .ToList().Count > 0;
        }

        public string GetImageSource(VehicleImage img)
        {
            string imgDataURL = "data:image;base64," + Convert.ToBase64String(img.ImageData);
            return imgDataURL;
        }
    }
}
