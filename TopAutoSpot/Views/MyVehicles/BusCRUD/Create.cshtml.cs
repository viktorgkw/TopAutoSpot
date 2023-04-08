namespace TopAutoSpot.Views.MyVehicles.BusCRUD
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    using TopAutoSpot.Data;
    using TopAutoSpot.Data.Models;

    /// <summary>
    /// A page model class that handles the creation of a new bus listing. Requires authorization.
    /// </summary>
    [Authorize]
    public class CreateModel : PageModel
    {
        protected readonly ApplicationDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateModel"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Called when the "Create" page is loaded via HTTP GET request.
        /// </summary>
        /// <returns>The page.</returns>
        public IActionResult OnGet()
        {
            if (User?.Identity?.Name == null)
            {
                return RedirectToPage("/Index");
            }

            return Page();
        }

        /// <summary>
        /// The bus object to be created, bound to the Razor page form. 
        /// </summary>
        [BindProperty]
        public Bus Bus { get; set; } = default!;

        /// <summary>
        /// The vehicle image object bound to the Razor page form.
        /// </summary>
        public VehicleImage VehicleImage { get; set; } = default!;

        /// <summary>
        /// Handles the HTTP POST request to create a new bus listing.
        /// </summary>
        /// <param name="Images">A list of form files representing the images to be associated with the listing.</param>
        /// <returns>The "MyVehicles/Index" page on success or "NotFound" on failure.</returns>
        public IActionResult OnPost(List<IFormFile> Images)
        {
            if (!ModelState.IsValid || !_context.Buses.Any() || Bus == null)
            {
                return RedirectToPage("/NotFound");
            }

            Bus.CreatedBy = _context.Users
                .First(u => u.UserName == User.Identity!.Name).Id;
            _context.Buses.Add(Bus);
            _context.SaveChanges();

            AddImagesToVehicle(Images, Bus.Id);

            return RedirectToPage("/MyVehicles/Index");
        }

        /// <summary>
        /// Adds the provided images to the newly created vehicle listing.
        /// </summary>
        /// <param name="images">A list of form files representing the images to be associated with the listing.</param>
        /// <param name="vehicleId">The ID of the newly created vehicle listing.</param>
        private void AddImagesToVehicle(List<IFormFile> images, string vehicleId)
        {
            images = images
                .Where(i =>
                    i.FileName.ToLower().EndsWith(".png") ||
                    i.FileName.ToLower().EndsWith(".jpeg") ||
                    i.FileName.ToLower().EndsWith(".jpg"))
                .ToList();

            if (images.Count > 0)
            {
                foreach (IFormFile image in images)
                {
                    using MemoryStream ms = new();
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
}
