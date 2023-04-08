namespace TopAutoSpot.Views.AdministratorViews
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;

    using TopAutoSpot.Data;
    using TopAutoSpot.Data.Models;

    /// <summary>
    /// Page model for managing auctions.
    /// </summary>
    [Authorize]
    public class ManageAuctionsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ManageAuctionsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public List<Auction> Auctions { get; set; } = null!;

        /// <summary>
        /// HTTP GET method for displaying the auctions.
        /// </summary>
        /// <returns>The page to be displayed.</returns>
        public async Task<IActionResult> OnGetAsync()
        {
            if (User.IsInRole("Administrator"))
            {
                Auctions = await _context.Auctions
                    .AsNoTracking()
                    .ToListAsync();

                return Page();
            }

            return RedirectToPage("/NotFound");
        }

        /// <summary>
        /// Retrieves the image data for the given auction ID.
        /// </summary>
        /// <param name="auctionId">The ID of the auction.</param>
        /// <returns>The image data as a base64-encoded string.</returns>
        public string GetAuctionImage(string auctionId)
        {
            string carId = _context.Auctions
                .AsNoTracking()
                .First(a => a.Id == auctionId)
                .VehicleId;

            byte[] data = _context.VehicleImages
                .AsNoTracking()
                .First(i => i.VehicleId == carId)
                .ImageData;

            string imgDataURL = "data:image;base64," + Convert.ToBase64String(data);
            return imgDataURL;
        }
    }
}
