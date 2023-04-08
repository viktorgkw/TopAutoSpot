namespace TopAutoSpot.Views.VehiclePreview
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;

    using TopAutoSpot.Data;
    using TopAutoSpot.Data.Models;
    using TopAutoSpot.Data.Models.Enums;

    /// <summary>
    /// Page model for displaying a preview of a Bus listing.
    /// </summary>
    public class BusPreviewModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public BusPreviewModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Bus Bus { get; set; } = default!;

        public List<VehicleImage> Images { get; set; } = null!;

        /// <summary>
        /// Handles the GET request and returns the page to display the Bus preview.
        /// </summary>
        /// <param name="id">The ID of the bus to display.</param>
        /// <returns>The page to display the bus preview.</returns>
        public IActionResult OnGet(string id)
        {
            if (id == null || !_context.Buses.Any())
            {
                return RedirectToPage("/NotFound");
            }

            Bus? bus = _context.Buses
                .AsNoTracking()
                .FirstOrDefault(b => b.Id == id);

            User foundUser = _context.Users
                .AsNoTracking()
                .First(u => u.UserName == User.Identity!.Name);

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
                .AsNoTracking()
                .Where(img => img.VehicleId == bus.Id)
                .ToList();

            return Page();
        }

        
        public string GetOwnerNumber()
        {
            User foundUser = _context.Users
                .AsNoTracking()
                .First(u => u.Id == Bus.CreatedBy);

            return foundUser.PhoneNumber!;
        }

        /// <summary>
        /// Gets the full name of the owner of the bus.
        /// </summary>
        /// <returns>The full name of the bus owner.</returns>
        public string GetOwnerFullName()
        {
            User foundUser = _context.Users
                .AsNoTracking()
                .First(u => u.Id == Bus.CreatedBy);

            return foundUser.FirstName + " " + foundUser.LastName;
        }

        /// <summary>
        /// Gets the data URL of the main image of the bus.
        /// </summary>
        /// <returns>The data URL of the main image of the bus.</returns>
        public string GetImage()
        {
            byte[] data = _context.VehicleImages
                .AsNoTracking()
                .Where(img => img.VehicleId == Bus.Id)
                .First()
                .ImageData;

            string imgDataURL = "data:image;base64," + Convert.ToBase64String(data);
            return imgDataURL;
        }

        /// <summary>
        /// Checks if the bus has any images.
        /// </summary>
        /// <returns>True if the bus has any images, false otherwise.</returns>
        public bool HasAnyImages()
        {
            return _context.VehicleImages
                .AsNoTracking()
                .Where(img => img.VehicleId == Bus.Id)
                .ToList().Count > 0;
        }

        /// <summary>
        /// Gets the data URL of an image of the bus.
        /// </summary>
        /// <param name="img">The image of the bus to get the data URL for.</param>
        /// <returns>The data URL of the image of the bus.</returns>
        public string GetImageSource(VehicleImage img)
        {
            string imgDataURL = "data:image;base64," + Convert.ToBase64String(img.ImageData);
            return imgDataURL;
        }
    }
}
