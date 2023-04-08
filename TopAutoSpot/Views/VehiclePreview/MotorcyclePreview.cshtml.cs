namespace TopAutoSpot.Views.VehiclePreview
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;

    using TopAutoSpot.Data;
    using TopAutoSpot.Data.Models;
    using TopAutoSpot.Data.Models.Enums;

    /// <summary>
    /// Page model for displaying a preview of a Motorcycle listing.
    /// </summary>
    public class MotorcyclePreviewModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public MotorcyclePreviewModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Motorcycle Motorcycle { get; set; } = default!;

        public List<VehicleImage> Images { get; set; } = null!;

        /// <summary>
        /// Gets the phone number of the owner of the motorcycle.
        /// </summary>
        /// <returns>The phone number of the motorcycle owner.</returns>
        public IActionResult OnGet(string id)
        {
            if (id == null || !_context.Motorcycles.Any())
            {
                return RedirectToPage("/NotFound");
            }

            Motorcycle? moto = _context.Motorcycles
                .AsNoTracking()
                .FirstOrDefault(m => m.Id == id);

            User foundUser = _context.Users
                .AsNoTracking()
                .First(u => u.UserName == User.Identity!.Name);

            if (moto == null)
            {
                return RedirectToPage("/NotFound");
            }
            else if (moto.Status != ListingStatusTypes.Active.ToString() && moto.CreatedBy != foundUser.Id)
            {
                return RedirectToPage("/MyVehicles/Index");
            }
            else
            {
                Motorcycle = moto;
            }

            Images = _context.VehicleImages
                .AsNoTracking()
                .Where(img => img.VehicleId == moto.Id)
                .ToList();

            return Page();
        }

        /// <summary>
        /// Gets the full name of the owner of the motorcycle.
        /// </summary>
        /// <returns>The full name of the motorcycle owner.</returns>
        public string GetOwnerNumber()
        {
            User foundUser = _context.Users
                .AsNoTracking()
                .First(u => u.Id == Motorcycle.CreatedBy);

            return foundUser.PhoneNumber!;
        }

        /// <summary>
        /// Gets the data URL of the main image of the motorcycle.
        /// </summary>
        /// <returns>The data URL of the main image of the motorcycle.</returns>
        public string GetOwnerFullName()
        {
            User foundUser = _context.Users
                .AsNoTracking()
                .First(u => u.Id == Motorcycle.CreatedBy);

            return foundUser.FirstName + " " + foundUser.LastName;
        }

        /// <summary>
        /// Gets the data URL of the main image of the motorcycle.
        /// </summary>
        /// <returns>The data URL of the main image of the motorcycle.</returns>
        public string GetImage()
        {
            byte[] data = _context.VehicleImages
                .AsNoTracking()
                .Where(img => img.VehicleId == Motorcycle.Id)
                .First()
                .ImageData;

            string imgDataURL = "data:image;base64," + Convert.ToBase64String(data);
            return imgDataURL;
        }

        /// <summary>
        /// Checks if the motorcycle has any images.
        /// </summary>
        /// <returns>True if the motorcycle has any images, false otherwise.</returns>
        public bool HasAnyImages()
        {
            return _context.VehicleImages
                .AsNoTracking()
                .Where(img => img.VehicleId == Motorcycle.Id)
                .ToList().Count > 0;
        }

        /// <summary>
        /// Gets the data URL of an image of the motorcycle.
        /// </summary>
        /// <param name="img">The image of the motorcycle to get the data URL for.</param>
        /// <returns>The data URL of the image of the motorcycle.</returns>
        public string GetImageSource(VehicleImage img)
        {
            string imgDataURL = "data:image;base64," + Convert.ToBase64String(img.ImageData);
            return imgDataURL;
        }
    }
}
