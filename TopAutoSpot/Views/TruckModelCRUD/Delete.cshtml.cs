using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TopAutoSpot.Data;
using TopAutoSpot.Data.Entities;

namespace TopAutoSpot.Views.TruckModelCRUD
{
    public class DeleteModel : PageModel
    {
        private readonly TopAutoSpot.Data.ApplicationDbContext _context;

        public DeleteModel(TopAutoSpot.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
      public Truck Truck { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null || _context.Trucks == null)
            {
                return NotFound();
            }

            var truck = await _context.Trucks.FirstOrDefaultAsync(m => m.Id == id);

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

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null || _context.Trucks == null)
            {
                return NotFound();
            }
            var truck = await _context.Trucks.FindAsync(id);

            if (truck != null)
            {
                Truck = truck;
                _context.Trucks.Remove(Truck);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
