using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopAutoSpot.Data.Entities;
using TopAutoSpot.Data;
using Microsoft.EntityFrameworkCore;

namespace TopAutoSpot.Views.VehiclePreview
{
    public class BoatPreviewModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public BoatPreviewModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Boat Boat { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null || _context.Boats == null)
            {
                return NotFound();
            }

            var boat = await _context.Boats.FirstOrDefaultAsync(b => b.Id == id);
            if (boat == null)
            {
                return NotFound();
            }
            else
            {
                Boat = boat;
            }
            return Page();
        }

        public string GetOwnerNumber()
        {
            var foundUser = _context.Users
                .First(u => u.Id == Boat.CreatedBy);

            return foundUser.PhoneNumber;
        }

        public string GetOwnerFullName()
        {
            var foundUser = _context.Users
                .First(u => u.Id == Boat.CreatedBy);

            return foundUser.FirstName + " " + foundUser.LastName;
        }
    }
}
