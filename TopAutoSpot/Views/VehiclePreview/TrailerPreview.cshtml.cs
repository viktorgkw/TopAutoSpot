using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopAutoSpot.Data;
using Microsoft.EntityFrameworkCore;
using TopAutoSpot.Models.Utilities;
using TopAutoSpot.Models;

namespace TopAutoSpot.Views.VehiclePreview
{
    public class TrailerPreviewModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public TrailerPreviewModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Trailer Trailer { get; set; } = default!;
        public List<VehicleImage> Images { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null || _context.Trailers == null)
            {
                return RedirectToPage("/NotFound");
            }

            var trailer = await _context.Trailers.FirstOrDefaultAsync(t => t.Id == id);
            var foundUser = await _context.Users.FirstAsync(u => u.UserName == User.Identity.Name);

            if (trailer == null)
            {
                return RedirectToPage("/NotFound");
            }
            else if (trailer.Status != StatusTypes.Active.ToString() &&
                trailer.CreatedBy != foundUser.Id)
            {
                return RedirectToPage("/MyVehicles/Index");
            }
            else
            {
                Trailer = trailer;
            }

            Images = _context.VehicleImages.Where(img => img.VehicleId == trailer.Id).ToList();
            return Page();
        }

        public string GetOwnerNumber()
        {
            var foundUser = _context.Users
                .First(u => u.Id == Trailer.CreatedBy);

            return foundUser.PhoneNumber;
        }

        public string GetOwnerFullName()
        {
            var foundUser = _context.Users
                .First(u => u.Id == Trailer.CreatedBy);

            return foundUser.FirstName + " " + foundUser.LastName;
        }

        public string GetImage()
        {
            var data = _context.VehicleImages.Where(img => img.VehicleId == Trailer.Id).First().ImageData;
            string imgDataURL = "data:image;base64," + Convert.ToBase64String(data);
            return imgDataURL;
        }

        public bool HasAnyImages()
        {
            return _context.VehicleImages.Where(img => img.VehicleId == Trailer.Id).ToList().Count > 0;
        }

        public string GetImageSource(VehicleImage img)
        {
            string imgDataURL = "data:image;base64," + Convert.ToBase64String(img.ImageData);
            return imgDataURL;
        }
    }
}
