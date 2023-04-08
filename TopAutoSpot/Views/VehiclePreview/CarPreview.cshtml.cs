namespace TopAutoSpot.Views.VehiclePreview
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;

    using TopAutoSpot.Data;
    using TopAutoSpot.Data.Models;
    using TopAutoSpot.Data.Models.Enums;

    /// <summary>
    /// Page model for displaying a preview of a Car listing.
    /// </summary>
    public class CarPreviewModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CarPreviewModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Car Car { get; set; } = default!;

        public List<VehicleImage> Images { get; set; } = null!;

        /// <summary>
        /// Handles the GET request and returns the page to display the Car preview.
        /// </summary>
        /// <param name="id">The ID of the car to display.</param>
        /// <returns>The page to display the car preview.</returns>
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

        /// <summary>
        /// Gets the phone number of the owner of the car.
        /// </summary>
        /// <returns>The phone number of the car owner.</returns>
        public string GetOwnerNumber()
        {
            User foundUser = _context.Users
                .AsNoTracking()
                .First(u => u.Id == Car.CreatedBy);

            return foundUser.PhoneNumber!;
        }

        /// <summary>
        /// Gets the full name of the owner of the car.
        /// </summary>
        /// <returns>The full name of the car owner.</returns>
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

        /// <summary>
        /// Gets the data URL of the main image of the car.
        /// </summary>
        /// <returns>The data URL of the main image of the car.</returns>
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

        /// <summary>
        /// Checks if the car has any images.
        /// </summary>
        /// <returns>True if the car has any images, false otherwise.</returns>
        public bool HasAnyImages()
        {
            return _context.VehicleImages
                .AsNoTracking()
                .Where(img => img.VehicleId == Car.Id)
                .ToList().Count > 0;
        }

        /// <summary>
        /// Gets the data URL of an image of the car.
        /// </summary>
        /// <param name="img">The image of the car to get the data URL for.</param>
        /// <returns>The data URL of the image of the car.</returns>
        public string GetImageSource(VehicleImage img)
        {
            string imgDataURL = "data:image;base64," + Convert.ToBase64String(img.ImageData);
            return imgDataURL;
        }
    }
}