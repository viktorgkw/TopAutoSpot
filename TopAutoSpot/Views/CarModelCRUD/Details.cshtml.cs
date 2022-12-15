﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TopAutoSpot.Data.Entities;

namespace TopAutoSpot.Views.CarModelCRUD
{
    public class DetailsModel : PageModel
    {
        private readonly TopAutoSpot.Data.ApplicationDbContext _context;

        public DetailsModel(TopAutoSpot.Data.ApplicationDbContext context)
        {
            _context = context;
        }

      public Car Car { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null || _context.Cars == null)
            {
                return NotFound();
            }

            var car = await _context.Cars.FirstOrDefaultAsync(m => m.Id == id);
            if (car == null)
            {
                return NotFound();
            }
            else 
            {
                Car = car;
            }
            return Page();
        }
    }
}