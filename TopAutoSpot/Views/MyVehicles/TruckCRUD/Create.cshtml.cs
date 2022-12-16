using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TopAutoSpot.Data;
using TopAutoSpot.Data.Entities;

namespace TopAutoSpot.Views.MyVehicles.TruckCRUD
{
    public class CreateModel : PageModel
    {
        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        protected readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Truck Truck { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || _context.Trucks == null || Truck == null)
            {
                var errors = ModelState.Where(a => a.Value.Errors.Count > 0)
                .Select(b => new { b.Key, b.Value.Errors })
                .ToArray();
                return Page();
            }

            Truck.CreatedBy = _context.Users.FirstAsync(u => u.UserName == User.Identity.Name).Result.Id;
            _context.Trucks.Add(Truck);
            await _context.SaveChangesAsync();

            return RedirectToPage("/MyVehicles/Index");
        }
    }
}
