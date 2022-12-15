using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TopAutoSpot.Data;
using TopAutoSpot.Data.Entities;

namespace TopAutoSpot.Views.BoatModelCRUD
{
    public class DetailsModel : PageModel
    {
        private readonly TopAutoSpot.Data.ApplicationDbContext _context;

        public DetailsModel(TopAutoSpot.Data.ApplicationDbContext context)
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

            var boat = await _context.Boats.FirstOrDefaultAsync(m => m.Id == id);
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
    }
}
