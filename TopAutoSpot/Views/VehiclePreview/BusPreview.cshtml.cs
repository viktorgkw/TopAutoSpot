using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopAutoSpot.Data.Entities;
using TopAutoSpot.Data;
using Microsoft.EntityFrameworkCore;

namespace TopAutoSpot.Views.VehiclePreview
{
    public class BusPreviewModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public BusPreviewModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Bus Bus { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null || _context.Buses == null)
            {
                return NotFound();
            }

            var bus = await _context.Buses.FirstOrDefaultAsync(b => b.Id == id);
            if (bus == null)
            {
                return NotFound();
            }
            else
            {
                Bus = bus;
            }
            return Page();
        }

        public string GetOwnerNumber()
        {
            var foundUser = _context.Users
                .First(u => u.Id == Bus.CreatedBy);

            return foundUser.PhoneNumber;
        }

        public string GetOwnerFullName()
        {
            var foundUser = _context.Users
                .First(u => u.Id == Bus.CreatedBy);

            return foundUser.FirstName + " " + foundUser.LastName;
        }
    }
}
