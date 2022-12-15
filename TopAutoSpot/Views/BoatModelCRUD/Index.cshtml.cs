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
    public class IndexModel : PageModel
    {
        private readonly TopAutoSpot.Data.ApplicationDbContext _context;

        public IndexModel(TopAutoSpot.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Boat> Boat { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Boats != null)
            {
                Boat = await _context.Boats.ToListAsync();
            }
        }
    }
}
