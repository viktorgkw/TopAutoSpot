namespace TopAutoSpot.Views.MyVehicles.MotorcycleCRUD
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    using TopAutoSpot.Data;
    using TopAutoSpot.Data.Models;

    /// <summary>
    /// This page model handles the deletion of a motorcycle by the owner of the motorcycle.
    /// </summary>
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Motorcycle Motorcycle { get; set; } = default!;

        public List<VehicleImage> Images { get; set; } = null!;

        /// <summary>
        /// Handles the GET request for the Delete page by verifying that the motorcycle exists and is owned by the current user.
        /// </summary>
        /// <param name="id">The ID of the motorcycle to be deleted.</param>
        /// <returns>The Delete page or a redirect to the Index page if the motorcycle is not found.</returns>
        public IActionResult OnGet(string id)
        {
            if (id == null || !_context.Motorcycles.Any())
            {
                return RedirectToPage("/NotFound");
            }

            Motorcycle? motorcycle = _context.Motorcycles.FirstOrDefault(m => m.Id == id);
            User foundUser = _context.Users.First(u => u.UserName == User.Identity!.Name);

            if (motorcycle == null)
            {
                return RedirectToPage("/NotFound");
            }
            else if (motorcycle.CreatedBy != foundUser.Id)
            {
                return RedirectToPage("/MyVehicles/Index");
            }
            else
            {
                Motorcycle = motorcycle;
            }

            Images = _context.VehicleImages
                .Where(img => img.VehicleId == motorcycle.Id)
                .ToList();

            return Page();
        }

        /// <summary>
        /// Handles the POST request for the Delete page by removing the motorcycle and associated images from the database.
        /// </summary>
        /// <param name="id">The ID of the motorcycle to be deleted.</param>
        /// <returns>A redirect to the Index page.</returns>
        public async Task<IActionResult> OnPost(string id)
        {
            if (id == null || !_context.Motorcycles.Any())
            {
                return RedirectToPage("/Index");
            }

            Motorcycle? motorcycle = await _context.Motorcycles.FindAsync(id);

            if (motorcycle != null)
            {
                RemoveVehicleImages(motorcycle.Id);

                Motorcycle = motorcycle;
                _context.Motorcycles.Remove(Motorcycle);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/MyVehicles/Index");
        }

        /// <summary>
        /// Removes all images associated with the given vehicle ID from the database.
        /// </summary>
        /// <param name="vehicleId">The ID of the vehicle whose images should be removed.</param>
        private void RemoveVehicleImages(string vehicleId)
        {
            List<VehicleImage> images = _context.VehicleImages
                .Where(i => i.VehicleId == vehicleId)
                .ToList();

            if (images.Count > 0)
            {
                foreach (VehicleImage? image in images)
                {
                    _context.VehicleImages.Remove(image);
                    _context.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Returns the data URL for the given vehicle image, which can be used to display the image in the browser.
        /// </summary>
        /// <param name="img">The vehicle image to display.</param>
        /// <returns>The data URL for the image.</returns>
        public string GetImageSource(VehicleImage img)
        {
            string imgDataURL = "data:image;base64," + Convert.ToBase64String(img.ImageData);
            return imgDataURL;
        }
    }
}
