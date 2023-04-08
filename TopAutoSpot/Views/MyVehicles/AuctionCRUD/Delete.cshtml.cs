namespace TopAutoSpot.Views.MyVehicles.AuctionCRUD
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    using TopAutoSpot.Data;
    using TopAutoSpot.Data.Models;

    /// <summary>
    /// The DeleteModel class is a PageModel used for deleting Auction items.
    /// This class is decorated with the [Authorize] attribute to ensure that only authenticated users can access it.
    /// </summary>
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Initializes a new instance of the DeleteModel class with the specified ApplicationDbContext.
        /// </summary>
        /// <param name="context">The ApplicationDbContext instance to use for data access.</param>
        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets or sets the Auction instance to be deleted.
        /// This property is decorated with the [BindProperty] attribute to indicate it will be bound to the request data.
        /// </summary>
        [BindProperty]
        public Auction Auction { get; set; } = default!;

        /// <summary>
        /// Handles GET requests for the Delete page.
        /// Retrieves the Auction object to be deleted and ensures the current user is authorized to delete it.
        /// </summary>
        /// <param name="id">The ID of the Auction object to delete.</param>
        /// <returns>An IActionResult representing the response to the GET request.</returns>
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
        /// Handles POST requests for the Delete page.
        /// Deletes the Auction object and saves the changes to the database.
        /// </summary>
        /// <param name="id">The ID of the Auction object to delete.</param>
        /// <returns>An IActionResult representing the response to the POST request.</returns>
        public IActionResult OnPost(string id)
        {
            if (id == null || _context.Auctions.Count() == 0)
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
