namespace TopAutoSpot.Views.MyVehicles.BusCRUD
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;

    using TopAutoSpot.Data;
    using TopAutoSpot.Data.Models;

    /// <summary>
    /// Controller for editing a Bus in the application, with authorization required.
    /// </summary>
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Bus to be edited, bound to form data.
        /// </summary>
        [BindProperty]
        public Bus Bus { get; set; } = default!;

        /// <summary>
        /// Vehicle image associated with the Bus to be edited.
        /// </summary>
        public VehicleImage VehicleImage { get; set; } = default!;

        /// <summary>
        /// Gets the Bus to be edited and verifies that the current user has permission to edit it.
        /// </summary>
        /// <param name="id">The ID of the Bus to be edited.</param>
        /// <returns>The edit page if the Bus can be edited, otherwise a redirect to the appropriate page.</returns>
        public IActionResult OnGet(string id)
        {
            if (id == null || _context.Buses.Count() == 0)
            {
                return RedirectToPage("/NotFound");
            }

            Bus? bus = _context.Buses
                .FirstOrDefault(b => b.Id == id);
            User foundUser = _context.Users
                .First(u => u.UserName == User.Identity!.Name);

            if (bus == null)
            {
                return RedirectToPage("/NotFound");
            }
            else if (bus.CreatedBy != foundUser.Id)
            {
                return RedirectToPage("/MyVehicles/Index");
            }
            else
            {
                Bus = bus;
            }

            return Page();
        }

        /// <summary>
        /// Updates the Bus and its associated images with the submitted form data.
        /// </summary>
        /// <param name="Images">List of form files containing the updated vehicle images.</param>
        /// <returns>A redirect to the user's vehicles index page.</returns>
        public IActionResult OnPost(List<IFormFile> Images)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Bus).State = EntityState.Modified;

            try
            {
                AddImagesToVehicle(Images, Bus.Id);
                _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BusExists(Bus.Id))
                {
                    return RedirectToPage("/Index");
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("/MyVehicles/Index");
        }

        /// <summary>
        /// Checks whether a Bus with the specified ID exists in the database.
        /// </summary>
        /// <param name="id">The ID of the Bus to check.</param>
        /// <returns>True if a Bus with the specified ID exists, otherwise false.</returns>
        private bool BusExists(string id)
        {
            return (_context.Buses?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        /// <summary>
        /// Adds the given images to the specified vehicle, replacing any existing images.
        /// </summary>
        /// <param name="images">The list of images to add to the vehicle.</param>
        /// <param name="vehicleId">The ID of the vehicle to add the images to.</param>
        private void AddImagesToVehicle(List<IFormFile> images, string vehicleId)
        {
            images = FilterImages(images);

            if (images.Count > 0)
            {
                RemoveExistingVehicleImages(vehicleId);

                foreach (IFormFile image in images)
                {
                    using (MemoryStream ms = new())
                    {
                        image.CopyTo(ms);

                        VehicleImage vehicleImage = new()
                        {

                            Id = Guid.NewGuid().ToString(),
                            ImageName = image.FileName,
                            ImageData = ms.ToArray(),
                            VehicleId = vehicleId,
                        };

                        _context.VehicleImages.Add(vehicleImage);
                        _context.SaveChanges();
                    }
                }
            }
        }

        /// <summary>
        /// Removes all vehicle images associated with the specified vehicle.
        /// </summary>
        /// <param name="vehicleId">The ID of the vehicle to remove images for.</param>
        private void RemoveExistingVehicleImages(string vehicleId)
        {
            List<VehicleImage> foundImages = _context.VehicleImages
                .Where(i => i.VehicleId == vehicleId)
                .ToList();

            foreach (VehicleImage? img in foundImages)
            {
                _context.Remove(img);
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Filters the given list of images to include only PNG, JPEG, and JPG files.
        /// </summary>
        /// <param name="images">The list of images to filter.</param>
        /// <returns>The filtered list of images.</returns>
        private static List<IFormFile> FilterImages(List<IFormFile> images)
        {
            images = images
                .Where(i =>
                    i.FileName.ToLower().EndsWith(".png") ||
                    i.FileName.ToLower().EndsWith(".jpeg") ||
                    i.FileName.ToLower().EndsWith(".jpg"))
                .ToList();

            return images;
        }
    }
}
