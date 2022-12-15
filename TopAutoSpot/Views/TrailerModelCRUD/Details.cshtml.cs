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
    public class DetailsModel : PageModel
    {
        private readonly TopAutoSpot.Data.ApplicationDbContext _context;

        public DetailsModel(TopAutoSpot.Data.ApplicationDbContext context)
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
    }
}
