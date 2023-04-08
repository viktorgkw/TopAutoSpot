namespace TopAutoSpot.Views.VehiclePreview
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;

    using TopAutoSpot.Data;
    using TopAutoSpot.Data.Models;
    using TopAutoSpot.Data.Models.Enums;

    /// <summary>
    /// Page model for displaying a preview of a Truck listing.
    /// </summary>
    public class TruckPreviewModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public TruckPreviewModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Truck Truck { get; set; } = default!;

        public List<VehicleImage> Images { get; set; } = null!;

        /// <summary>
        /// Handles the GET request and returns the page to display the truck preview.
        /// </summary>
        /// <param name="id">The ID of the truck to display.</param>
        /// <returns>The page to display the truck preview.</returns>
        public IActionResult OnGet(string id)
        {
            if (id == null || !_context.Trucks.Any())
            {
                return RedirectToPage("/NotFound");
            }

            Truck? truck = _context.Trucks
                .AsNoTracking()
                .FirstOrDefault(t => t.Id == id);

            User foundUser = _context.Users
                .AsNoTracking()
                .First(u => u.UserName == User.Identity!.Name);

            if (truck == null)
            {
                return RedirectToPage("/NotFound");
            }
            else if (truck.Status != ListingStatusTypes.Active.ToString() &&
                truck.CreatedBy != foundUser.Id)
            {
                return RedirectToPage("/MyVehicles/Index");
            }
            else
            {
                Truck = truck;
            }

            Images = _context.VehicleImages
                .AsNoTracking()
                .Where(img => img.VehicleId == truck.Id)
                .ToList();

            return Page();
        }

        /// <summary>
        /// Gets the phone number of the owner of the truck.
        /// </summary>
        /// <returns>The phone number of the truck owner.</returns>
        public string GetOwnerNumber()
        {
            User foundUser = _context.Users
                .AsNoTracking()
                .First(u => u.Id == Truck.CreatedBy);

            return foundUser.PhoneNumber!;
        }

        /// <summary>
        /// Gets the full name of the owner of the truck.
        /// </summary>
        /// <returns>The full name of the truck owner.</returns>
        public string GetOwnerFullName()
        {
            User foundUser = _context.Users
                .AsNoTracking()
                .First(u => u.Id == Truck.CreatedBy);

            return foundUser.FirstName + " " + foundUser.LastName;
        }


        /// <summary>
        /// Gets the data URL of the main image of the truck.
        /// </summary>
        /// <returns>The data URL of the main image of the truck.</returns>
        public string GetImage()
        {
            byte[] data = _context.VehicleImages
                .AsNoTracking()
                .Where(img => img.VehicleId == Truck.Id)
                .First()
                .ImageData;

            string imgDataURL = "data:image;base64," + Convert.ToBase64String(data);
            return imgDataURL;
        }

        /// <summary>
        /// Checks if the truck has any images.
        /// </summary>
        /// <returns>True if the truck has any images, false otherwise.</returns>
        public bool HasAnyImages()
        {
            return _context.VehicleImages
                .AsNoTracking()
                .Where(img => img.VehicleId == Truck.Id)
                .ToList().Count > 0;
        }

        /// <summary>
        /// Gets the data URL of an image of the truck.
        /// </summary>
        /// <param name="img">The image of the truck to get the data URL for.</param>
        /// <returns>The data URL of the image of the truck.</returns>
        public string GetImageSource(VehicleImage img)
        {
            string imgDataURL = "data:image;base64," + Convert.ToBase64String(img.ImageData);
            return imgDataURL;
        }
    }
}
