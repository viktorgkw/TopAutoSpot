using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopAutoSpot.Data.Entities;
using TopAutoSpot.Data;
using Microsoft.EntityFrameworkCore;

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

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null || _context.Trailers == null)
            {
                return NotFound();
            }

            var trailer = await _context.Trailers.FirstOrDefaultAsync(t => t.Id == id);
            if (trailer == null)
            {
                return NotFound();
            }
            else
            {
                Trailer = trailer;
            }
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
    }
}
