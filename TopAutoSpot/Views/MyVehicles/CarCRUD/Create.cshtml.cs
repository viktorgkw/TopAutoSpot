using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TopAutoSpot.Data;
using TopAutoSpot.Data.Entities;

namespace TopAutoSpot.Views.MyVehicles.CarCRUD
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
        public Car Car { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            Car.CreatedBy = _context.Users.FirstAsync(u => u.UserName == User.Identity.Name).Result.Id;
            if (!ModelState.IsValid || _context.Cars == null || Car == null)
            {
                var errors = ModelState.Where(a => a.Value.Errors.Count > 0)
                .Select(b => new { b.Key, b.Value.Errors })
                .ToArray();
                return Page();
            }

            _context.Cars.Add(Car);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
