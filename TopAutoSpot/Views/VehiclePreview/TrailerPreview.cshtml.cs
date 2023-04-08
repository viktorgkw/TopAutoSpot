namespace TopAutoSpot.Views.VehiclePreview
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;

    using TopAutoSpot.Data;
    using TopAutoSpot.Data.Models;
    using TopAutoSpot.Data.Models.Enums;

    /// <summary>
    /// Page model for displaying a preview of a Trailer listing.
    /// </summary>
    public class TrailerPreviewModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public TrailerPreviewModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Trailer Trailer { get; set; } = default!;

        public List<VehicleImage> Images { get; set; } = null!;

        /// <summary>
        /// Handles the GET request and returns the page to display the Trailer preview.
        /// </summary>
        /// <param name="id">The ID of the trailer to display.</param>
        /// <returns>The page to display the trailer preview.</returns>
        public IActionResult OnGet(string id)
        {
            if (id == null || !_context.Trailers.Any())
            {
                return RedirectToPage("/NotFound");
            }

            Trailer? trailer = _context.Trailers
                .AsNoTracking()
                .FirstOrDefault(t => t.Id == id);

            User foundUser = _context.Users
                .AsNoTracking()
                .First(u => u.UserName == User.Identity!.Name);

            if (trailer == null)
            {
                return RedirectToPage("/NotFound");
            }
            else if (trailer.Status != ListingStatusTypes.Active.ToString() &&
                trailer.CreatedBy != foundUser.Id)
            {
                return RedirectToPage("/MyVehicles/Index");
            }
            else
            {
                Trailer = trailer;
            }

            Images = _context.VehicleImages
                .AsNoTracking()
                .Where(img => img.VehicleId == trailer.Id)
                .ToList();

            return Page();
        }

        /// <summary>
        /// Gets the phone number of the owner of the trailer.
        /// </summary>
        /// <returns>The phone number of the trailer owner.</returns>
        public string GetOwnerNumber()
        {
            User foundUser = _context.Users
                .AsNoTracking()
                .First(u => u.Id == Trailer.CreatedBy);

            return foundUser.PhoneNumber!;
        }

        /// <summary>
        /// Gets the full name of the owner of the trailer.
        /// </summary>
        /// <returns>The full name of the trailer owner.</returns>
        public string GetOwnerFullName()
        {
            User foundUser = _context.Users
                .AsNoTracking()
                .First(u => u.Id == Trailer.CreatedBy);

            return foundUser.FirstName + " " + foundUser.LastName;
        }

        /// <summary>
        /// Gets the data URL of the main image of the trailer.
        /// </summary>
        /// <returns>The data URL of the main image of the trailer.</returns>
        public string GetImage()
        {
            byte[] data = _context.VehicleImages
                .AsNoTracking()
                .Where(img => img.VehicleId == Trailer.Id)
                .First()
                .ImageData;

            string imgDataURL = "data:image;base64," + Convert.ToBase64String(data);
            return imgDataURL;
        }

        /// <summary>
        /// Checks if the trailer has any images.
        /// </summary>
        /// <returns>True if the trailer has any images, false otherwise.</returns>
        public bool HasAnyImages()
        {
            return _context.VehicleImages
                .AsNoTracking()
                .Where(img => img.VehicleId == Trailer.Id)
                .ToList().Count > 0;
        }

        /// <summary>
        /// Gets the data URL of an image of the trailer.
        /// </summary>
        /// <param name="img">The image of the trailer to get the data URL for.</param>
        /// <returns>The data URL of the image of the trailer.</returns>
        public string GetImageSource(VehicleImage img)
        {
            string imgDataURL = "data:image;base64," + Convert.ToBase64String(img.ImageData);
            return imgDataURL;
        }
    }
}
