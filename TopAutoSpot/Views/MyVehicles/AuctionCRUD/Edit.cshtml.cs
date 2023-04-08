namespace TopAutoSpot.Views.MyVehicles.AuctionCRUD
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;

    using TopAutoSpot.Data;
    using TopAutoSpot.Data.Models;

    /// <summary>
    /// The EditModel class is a PageModel used for editing Auction items.
    /// This class is decorated with the [Authorize] attribute to ensure that only authenticated users can access it.
    /// </summary>
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

        /// <summary>
        /// Retrieves data for the specified Auction item.
        /// </summary>
        /// <param name="id">The ID of the Auction to retrieve.</param>
        public IActionResult OnGet(string id)
        {
            if (id == null || _context.Auctions.Count() == 0)
            {
                return RedirectToPage("/NotFound");
            }

            Auction? auction = _context.Auctions.FirstOrDefault(m => m.Id == id);
            User foundUser = _context.Users.First(u => u.UserName == User.Identity!.Name);

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

        /// <summary>
        /// Submits edited Auction data to the database.
        /// </summary>
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

        /// <summary>
        /// Checks if an Auction with a given ID exists.
        /// </summary>
        /// <param name="id">The ID of the Auction to check for.</param>
        private bool AuctionExists(string id)
        {
            return (_context.Auctions?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
