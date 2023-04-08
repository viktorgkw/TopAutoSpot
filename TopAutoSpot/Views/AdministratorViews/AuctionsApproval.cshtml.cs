namespace TopAutoSpot.Views.AdministratorViews
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;

    using TopAutoSpot.Data;
    using TopAutoSpot.Data.Models;
    using TopAutoSpot.Data.Models.Enums;

    /// <summary>
    /// Controller for managing auctions waiting for approval by the administrator.
    /// Requires the user to be an Administrator.
    /// </summary>
    [Authorize]
    public class AuctionsApprovalModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="AuctionsApprovalModel"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public AuctionsApprovalModel(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets or sets the list of auctions waiting for approval.
        /// </summary>
        public List<Auction> Auctions { get; set; } = null!;

        /// <summary>
        /// Handles GET requests to the auctions approval page.
        /// </summary>
        /// <returns>The page result.</returns>
        public IActionResult OnGet()
        {
            if (User.IsInRole("Administrator"))
            {
                Auctions = _context.Auctions
                    .AsNoTracking()
                    .Where(a => a.Status == AuctionStatusTypes.WaitingApproval.ToString())
                    .ToList();

                return Page();
            }

            return RedirectToPage("/NotFound");
        }

        /// <summary>
        /// Gets the image for the specified auction.
        /// </summary>
        /// <param name="auctionId">The ID of the auction.</param>
        /// <returns>The base64-encoded image data URL.</returns>
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
