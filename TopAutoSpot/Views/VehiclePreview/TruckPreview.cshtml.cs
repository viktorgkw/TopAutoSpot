using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopAutoSpot.Data.Entities;
using TopAutoSpot.Data;
using Microsoft.EntityFrameworkCore;

namespace TopAutoSpot.Views.VehiclePreview
{
    public class TruckPreviewModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public TruckPreviewModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Truck Truck { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null || _context.Trucks == null)
            {
                return NotFound();
            }

            var truck = await _context.Trucks.FirstOrDefaultAsync(t => t.Id == id);
            if (truck == null)
            {
                return NotFound();
            }
            else
            {
                Truck = truck;
            }
            return Page();
        }

        public string GetOwnerNumber()
        {
            var foundUser = _context.Users
                .First(u => u.Id == Truck.CreatedBy);

            return foundUser.PhoneNumber;
        }

        public string GetOwnerFullName()
        {
            var foundUser = _context.Users
                .First(u => u.Id == Truck.CreatedBy);

            return foundUser.FirstName + " " + foundUser.LastName;
        }
    }
}
