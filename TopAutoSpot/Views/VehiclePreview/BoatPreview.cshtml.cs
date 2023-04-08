namespace TopAutoSpot.Views.VehiclePreview
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;

    using TopAutoSpot.Data;
    using TopAutoSpot.Data.Models;
    using TopAutoSpot.Data.Models.Enums;

    /// <summary>
    /// Page model for displaying a preview of a Boat listing.
    /// </summary>
    public class BoatPreviewModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public BoatPreviewModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Boat Boat { get; set; } = default!;

        public List<VehicleImage> Images { get; set; } = null!;

        /// <summary>
        /// Handles the GET request and returns the page to display the boat preview.
        /// </summary>
        /// <param name="id">The ID of the boat to display.</param>
        /// <returns>The page to display the boat preview.</returns>
        public IActionResult OnGet(string id)
        {
            if (id == null || _context.Boats.Count() == 0)
            {
                return RedirectToPage("/NotFound");
            }

            Boat? boat = _context.Boats
                .AsNoTracking()
                .FirstOrDefault(b => b.Id == id);

            User foundUser = _context.Users
                .AsNoTracking()
                .First(u => u.UserName == User.Identity!.Name);

            if (boat == null)
            {
                return RedirectToPage("/NotFound");
            }
            else if (boat.Status != ListingStatusTypes.Active.ToString() && boat.CreatedBy != foundUser.Id)
            {
                return RedirectToPage("/MyVehicles/Index");
            }
            else
            {
                Boat = boat;
            }

            Images = _context.VehicleImages
                .AsNoTracking()
                .Where(img => img.VehicleId == boat.Id)
                .ToList();

            return Page();
        }

        /// <summary>
        /// Gets the phone number of the owner of the boat.
        /// </summary>
        /// <returns>The phone number of the boat owner.</returns>
        public string GetOwnerNumber()
        {
            User foundUser = _context.Users
                .AsNoTracking()
                .First(u => u.Id == Boat.CreatedBy);

            return foundUser.PhoneNumber!;
        }

        /// <summary>
        /// Gets the full name of the owner of the boat.
        /// </summary>
        /// <returns>The full name of the boat owner.</returns>
        public string GetOwnerFullName()
        {
            User foundUser = _context.Users
                .AsNoTracking()
                .First(u => u.Id == Boat.CreatedBy);

            return foundUser.FirstName + " " + foundUser.LastName;
        }

        /// <summary>
        /// Gets the data URL of the main image of the boat.
        /// </summary>
        /// <returns>The data URL of the main image of the boat.</returns>
        public string GetImage()
        {
            byte[] data = _context.VehicleImages
                .AsNoTracking()
                .Where(img => img.VehicleId == Boat.Id)
                .First()
                .ImageData;

            string imgDataURL = "data:image;base64," + Convert.ToBase64String(data);
            return imgDataURL;
        }

        /// <summary>
        /// Checks if the boat has any images.
        /// </summary>
        /// <returns>True if the boat has any images, false otherwise.</returns>
        public bool HasAnyImages()
        {
            return _context.VehicleImages
                .AsNoTracking()
                .Where(img => img.VehicleId == Boat.Id)
                .ToList().Count > 0;
        }

        /// <summary>
        /// Gets the data URL of an image of the boat.
        /// </summary>
        /// <param name="img">The image of the boat to get the data URL for.</param>
        /// <returns>The data URL of the image of the boat.</returns>
        public string GetImageSource(VehicleImage img)
        {
            string imgDataURL = "data:image;base64," + Convert.ToBase64String(img.ImageData);
            return imgDataURL;
        }
    }
}
