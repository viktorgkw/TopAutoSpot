using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TopAutoSpot.Data;
using TopAutoSpot.Models;

namespace TopAutoSpot.Views.MyVehicles.AuctionCRUD
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Auction Auction { get; set; } = default!;

        public IActionResult OnGet(string id)
        {
            if (id == null || _context.Auctions == null)
            {
                return RedirectToPage("/NotFound");
            }

            Auction? auction = _context.Auctions.FirstOrDefault(m => m.Id == id);
            User foundUser = _context.Users.First(u => u.UserName == User.Identity.Name);

            if (auction == null)
            {
                return RedirectToPage("/NotFound");
            }
            else if (auction.AuctioneerId != foundUser.Id)
            {
                return RedirectToPage("/MyVehicles/Index");
            }
            else
            {
                Auction = auction;
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Auction).State = EntityState.Modified;

            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AuctionExists(Auction.Id))
                {
                    return RedirectToPage("/Index");
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("/MyVehicles/Index");
        }

        private bool AuctionExists(string id)
        {
            return (_context.Auctions?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
