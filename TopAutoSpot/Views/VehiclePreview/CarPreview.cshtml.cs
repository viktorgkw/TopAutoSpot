using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TopAutoSpot.Data;
using TopAutoSpot.Data.Entities;
using TopAutoSpot.Data.Entities.Utilities;

namespace TopAutoSpot.Views.VehiclePreview
{
    public class CarPreviewModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CarPreviewModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Car Car { get; set; } = default!;
        public List<VehicleImage> Images { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null || _context.Cars == null)
            {
                return RedirectToPage("/NotFound");
            }

            var car = await _context.Cars.FirstOrDefaultAsync(m => m.Id == id);
            var foundUser = await _context.Users.FirstAsync(u => u.UserName == User.Identity.Name);

            if (car == null)
            {
                return RedirectToPage("/NotFound");
            }
            else if (car.Status != StatusTypes.Active.ToString() && car.CreatedBy != foundUser.Id)
            {
                return RedirectToPage("/MyVehicles/Index");
            }
            else
            {
                Car = car;
            }

            Images = _context.VehicleImages.Where(img => img.VehicleId == car.Id).ToList();
            return Page();
        }

        public string GetOwnerNumber()
        {
            var foundUser = _context.Users
                .First(u => u.Id == Car.CreatedBy);

            return foundUser.PhoneNumber;
        }

        public string GetOwnerFullName()
        {
            var foundUser = _context.Users
                .First(u => u.Id == Car.CreatedBy);

            return foundUser.FirstName + " " + foundUser.LastName == " " ? foundUser.UserName :
                foundUser.FirstName + " " + foundUser.LastName;
        }

        public string GetImage()
        {
            var data = _context.VehicleImages.Where(img => img.VehicleId == Car.Id).First().ImageData;
            string imgDataURL = "data:image;base64," + Convert.ToBase64String(data);
            return imgDataURL;
        }

        public bool HasAnyImages()
        {
            return _context.VehicleImages.Where(img => img.VehicleId == Car.Id).ToList().Count > 0;
        }

        public string GetImageSource(VehicleImage img)
        {
            string imgDataURL = "data:image;base64," + Convert.ToBase64String(img.ImageData);
            return imgDataURL;
        }
    }
}