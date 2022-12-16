using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TopAutoSpot.Data;
using TopAutoSpot.Data.Entities;

namespace TopAutoSpot.Views.MyVehicles.MotorcycleCRUD
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Motorcycle Motorcycle { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || _context.Motorcycles == null || Motorcycle == null)
            {
                return Page();
            }

            Motorcycle.CreatedBy = _context.Users
                .FirstAsync(u => u.UserName == User.Identity.Name).Result.Id;
            _context.Motorcycles.Add(Motorcycle);
            await _context.SaveChangesAsync();

            return RedirectToPage("/MyVehicles/Index");
        }
    }
}
