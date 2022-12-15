using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TopAutoSpot.Data;
using TopAutoSpot.Data.Entities;

namespace TopAutoSpot.Views.MotorcycleModelCRUD
{
    public class DeleteModel : PageModel
    {
        private readonly TopAutoSpot.Data.ApplicationDbContext _context;

        public DeleteModel(TopAutoSpot.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
      public Motorcycle Motorcycle { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null || _context.Motorcycles == null)
            {
                return NotFound();
            }

            var motorcycle = await _context.Motorcycles.FirstOrDefaultAsync(m => m.Id == id);

            if (motorcycle == null)
            {
                return NotFound();
            }
            else 
            {
                Motorcycle = motorcycle;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null || _context.Motorcycles == null)
            {
                return NotFound();
            }
            var motorcycle = await _context.Motorcycles.FindAsync(id);

            if (motorcycle != null)
            {
                Motorcycle = motorcycle;
                _context.Motorcycles.Remove(Motorcycle);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
