using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TopAutoSpot.Data;
using TopAutoSpot.Data.Models;
using TopAutoSpot.Data.Models.Enums;

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

        public IActionResult OnGet(string id)
        {
            if (id == null || _context.Trailers.Count() == 0)
            {
                return RedirectToPage("/NotFound");
            }

            Trailer? trailer = _context.Trailers
                .AsNoTracking()
                .FirstOrDefault(t => t.Id == id);

            User foundUser = _context.Users
                .AsNoTracking()
                .First(u => u.UserName == User.Identity.Name);

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

        public string GetOwnerNumber()
        {
            User foundUser = _context.Users
                .AsNoTracking()
                .First(u => u.Id == Trailer.CreatedBy);

            return foundUser.PhoneNumber;
        }

        public string GetOwnerFullName()
        {
            User foundUser = _context.Users
                .AsNoTracking()
                .First(u => u.Id == Trailer.CreatedBy);

            return foundUser.FirstName + " " + foundUser.LastName;
        }

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

        public bool HasAnyImages()
        {
            return _context.VehicleImages
                .AsNoTracking()
                .Where(img => img.VehicleId == Trailer.Id)
                .ToList().Count > 0;
        }

        public string GetImageSource(VehicleImage img)
        {
            string imgDataURL = "data:image;base64," + Convert.ToBase64String(img.ImageData);
            return imgDataURL;
        }
    }
}
