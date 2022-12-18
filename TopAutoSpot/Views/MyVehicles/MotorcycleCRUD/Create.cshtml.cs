using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TopAutoSpot.Data;
using TopAutoSpot.Data.Entities;

namespace TopAutoSpot.Views.MyVehicles.MotorcycleCRUD
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            if(User.Identity.Name == null)
            {
                return RedirectToPage("/Index");
            }

            return Page();
        }

        [BindProperty]
        public Motorcycle Motorcycle { get; set; } = default!;
        public VehicleImage VehicleImage { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync(List<IFormFile> Images)
        {
            if (!ModelState.IsValid || _context.Motorcycles == null || Motorcycle == null)
            {
                return RedirectToPage("/NotFound");
            }

            Motorcycle.CreatedBy = _context.Users
                .FirstAsync(u => u.UserName == User.Identity.Name).Result.Id;
            _context.Motorcycles.Add(Motorcycle);
            await _context.SaveChangesAsync();

            await AddImagesToVehicle(Images, Motorcycle.Id);

            return RedirectToPage("/MyVehicles/Index");
        }

        private async Task AddImagesToVehicle(List<IFormFile> images, string vehicleId)
        {
            
            images = images
                .Where(i =>
                    i.FileName.EndsWith(".png") ||
                    i.FileName.EndsWith(".jpeg") ||
                    i.FileName.EndsWith(".jpg"))
                .ToList();

            if (images.Count > 0)
            {
                foreach (IFormFile image in images)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        image.CopyTo(ms);

                        var vehicleImage = new VehicleImage()
                        {

                            Id = Guid.NewGuid().ToString(),
                            ImageName = image.FileName,
                            ImageData = ms.ToArray(),
                            VehicleId = vehicleId,
                        };

                        await _context.VehicleImages.AddAsync(vehicleImage);
                        await _context.SaveChangesAsync();
                    }
                }
            }
        }
    }
}
