﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using TopAutoSpot.Data;
using TopAutoSpot.Data.Entities;

namespace TopAutoSpot.Views.TruckModelCRUD
{
    public class CreateModel : PageModel
    {
        private readonly TopAutoSpot.Data.ApplicationDbContext _context;

        public CreateModel(TopAutoSpot.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Truck Truck { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.Trucks == null || Truck == null)
            {
                return Page();
            }

            _context.Trucks.Add(Truck);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
