using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TopAutoSpot.Data;
using TopAutoSpot.Data.Models;
using TopAutoSpot.Data.Models.Enums;

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
        public List<VehicleImage> Images { get; set; } = null!;

        public IActionResult OnGet(string id)
        {
            if (id == null || !_context.Cars.Any())
            {
                return RedirectToPage("/NotFound");
            }

            Car? car = _context.Cars
                .AsNoTracking()
                .FirstOrDefault(m => m.Id == id);

            User foundUser = _context.Users
                .AsNoTracking()
                .First(u => u.UserName == User.Identity!.Name);

            if (car == null)
            {
                return RedirectToPage("/NotFound");
            }
            else if (car.Status != ListingStatusTypes.Active.ToString() && car.CreatedBy != foundUser.Id)
            {
                return RedirectToPage("/MyVehicles/Index");
            }
            else
            {
                Car = car;
            }

            Images = _context.VehicleImages
                .AsNoTracking()
                .Where(img => img.VehicleId == car.Id)
                .ToList();

            return Page();
        }

        public string GetOwnerNumber()
        {
            User foundUser = _context.Users
                .AsNoTracking()
                .First(u => u.Id == Car.CreatedBy);

            return foundUser.PhoneNumber!;
        }

        public string GetOwnerFullName()
        {
            User foundUser = _context.Users
                .AsNoTracking()
                .First(u => u.Id == Car.CreatedBy);

            string? fullName = foundUser.FirstName + " " + foundUser.LastName == " "
                ? foundUser.UserName
                : foundUser.FirstName + " " + foundUser.LastName;

            return fullName!;
        }

        public string GetImage()
        {
            byte[] data = _context.VehicleImages
                .AsNoTracking()
                .Where(img => img.VehicleId == Car.Id)
                .First()
                .ImageData;

            string imgDataURL = "data:image;base64," + Convert.ToBase64String(data);
            return imgDataURL;
        }

        public bool HasAnyImages()
        {
            return _context.VehicleImages
                .AsNoTracking()
                .Where(img => img.VehicleId == Car.Id)
                .ToList().Count > 0;
        }

        public string GetImageSource(VehicleImage img)
        {
            string imgDataURL = "data:image;base64," + Convert.ToBase64String(img.ImageData);
            return imgDataURL;
        }
    }
}