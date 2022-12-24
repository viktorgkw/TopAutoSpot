using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopAutoSpot.Data;
using TopAutoSpot.Models;

namespace TopAutoSpot.Views.MyVehicles.AuctionCRUD
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
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

        public IActionResult OnPost(string id)
        {
            if (id == null || _context.Auctions == null)
            {
                return RedirectToPage("/Index");
            }
            Auction? auction = _context.Auctions.Find(id);

            if (auction != null)
            {
                Auction = auction;
                _context.Auctions.Remove(Auction);
                _context.SaveChanges();
            }

            return RedirectToPage("/MyVehicles/Index");
        }
    }
}
