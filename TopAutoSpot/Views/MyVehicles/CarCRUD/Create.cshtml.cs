using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopAutoSpot.Data;
using TopAutoSpot.Models;

namespace TopAutoSpot.Views.MyVehicles.CarCRUD
{
    [Authorize]
    public class CreateModel : PageModel
    {
        protected readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            if (User?.Identity?.Name == null)
            {
                return RedirectToPage("/Index");
            }

            return Page();
        }

        [BindProperty]
        public Car Car { get; set; } = default!;
        public VehicleImage VehicleImage { get; set; } = default!;

        public IActionResult OnPost(List<IFormFile> Images)
        {
            if (!ModelState.IsValid || _context.Cars.Count() == 0 || Car == null)
            {
                return RedirectToPage("/NotFound");
            }

            Car.CreatedBy = _context.Users.First(u => u.UserName == User.Identity.Name).Id;
            _context.Cars.Add(Car);
            _context.SaveChanges();

            AddImagesToVehicle(Images, Car.Id);

            return RedirectToPage("/MyVehicles/Index");
        }

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
                    using (MemoryStream ms = new MemoryStream())
                    {
                        image.CopyTo(ms);

                        VehicleImage vehicleImage = new VehicleImage()
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
}
