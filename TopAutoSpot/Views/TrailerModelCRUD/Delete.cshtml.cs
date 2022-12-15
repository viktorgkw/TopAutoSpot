using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TopAutoSpot.Data;
using TopAutoSpot.Data.Entities;

namespace TopAutoSpot.Views.TrailerModelCRUD
{
    public class DeleteModel : PageModel
    {
        private readonly TopAutoSpot.Data.ApplicationDbContext _context;

        public DeleteModel(TopAutoSpot.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
      public Trailer Trailer { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null || _context.Trailers == null)
            {
                return NotFound();
            }

            var trailer = await _context.Trailers.FirstOrDefaultAsync(m => m.Id == id);

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

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null || _context.Trailers == null)
            {
                return NotFound();
            }
            var trailer = await _context.Trailers.FindAsync(id);

            if (trailer != null)
            {
                Trailer = trailer;
                _context.Trailers.Remove(Trailer);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
