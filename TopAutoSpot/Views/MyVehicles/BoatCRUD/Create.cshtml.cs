using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TopAutoSpot.Data;
using TopAutoSpot.Data.Entities;

namespace TopAutoSpot.Views.MyVehicles.BoatCRUD
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
        public Boat Boat { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || _context.Boats == null || Boat == null)
            {
                var errors = ModelState.Where(a => a.Value.Errors.Count > 0)
                .Select(b => new { b.Key, b.Value.Errors })
                .ToArray();
                return Page();
            }

            Boat.CreatedBy = _context.Users
                .FirstAsync(u => u.UserName == User.Identity.Name).Result.Id;
            _context.Boats.Add(Boat);
            await _context.SaveChangesAsync();

            return RedirectToPage("/MyVehicles/Index");
        }
    }
}
