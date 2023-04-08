namespace TopAutoSpot.Views.MyVehicles.CarCRUD
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;

    using TopAutoSpot.Data;
    using TopAutoSpot.Data.Models;

    /// <summary>
    /// Page model for editing a car in the application, with authorization required.
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
        /// car to be edited, bound to form data.
        /// </summary>
        [BindProperty]
        public Car Car { get; set; } = default!;

        /// <summary>
        /// Vehicle image associated with the car to be edited.
        /// </summary>
        public VehicleImage VehicleImage { get; set; } = default!;

        /// <summary>
        /// Gets the car to be edited and verifies that the current user has permission to edit it.
        /// </summary>
        /// <param name="id">The ID of the car to be edited.</param>
        /// <returns>The edit page if the car can be edited, otherwise a redirect to the appropriate page.</returns>
        public IActionResult OnGet(string id)
        {
            if (id == null || _context.Cars.Count() == 0)
            {
                return RedirectToPage("/NotFound");
            }

            Car? car = _context.Cars.FirstOrDefault(m => m.Id == id);
            User foundUser = _context.Users.First(u => u.UserName == User.Identity!.Name);

            if (car == null)
            {
                return RedirectToPage("/NotFound");
            }
            else if (car.CreatedBy != foundUser.Id)
            {
                return RedirectToPage("/MyVehicles/Index");
            }
            else
            {
                Car = car;
            }

            return Page();
        }

        /// <summary>
        /// Updates the car and its associated images with the submitted form data.
        /// </summary>
        /// <param name="Images">List of form files containing the updated vehicle images.</param>
        /// <returns>A redirect to the user's vehicles index page.</returns>
        public IActionResult OnPost(List<IFormFile> Images)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Car).State = EntityState.Modified;

            try
            {
                AddImagesToVehicle(Images, Car.Id);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarExists(Car.Id))
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
        /// Checks whether a car with the specified ID exists in the database.
        /// </summary>
        /// <param name="id">The ID of the car to check.</param>
        /// <returns>True if a car with the specified ID exists, otherwise false.</returns>
        private bool CarExists(string id)
        {
            return (_context.Cars?.Any(e => e.Id == id)).GetValueOrDefault();
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
