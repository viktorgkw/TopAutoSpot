namespace TopAutoSpot.Views.VehiclePreview
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;

    using TopAutoSpot.Data;
    using TopAutoSpot.Data.Models;
    using TopAutoSpot.Data.Models.Enums;

    /// <summary>
    /// Page model for displaying a preview of a Auction listing.
    /// </summary>
    public class AuctionPreviewModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public AuctionPreviewModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public Auction Auction { get; set; } = default!;

        /// <summary>
        /// Handles the GET request and returns the page to display the Auction preview.
        /// </summary>
        /// <param name="id">The ID of the Auction to display.</param>
        /// <returns>The page to display the Auction preview.</returns>
        public IActionResult OnGet(string id)
        {
            if (id == null || !_context.Auctions.Any())
            {
                return RedirectToPage("/NotFound");
            }

            Auction? auction = _context.Auctions
                .AsNoTracking()
                .FirstOrDefault(b => b.Id == id);

            User foundUser = _context.Users
                .AsNoTracking()
                .First(u => u.UserName == User.Identity!.Name);

            if (auction == null)
            {
                return RedirectToPage("/NotFound");
            }
            else if (auction.Status != ListingStatusTypes.Active.ToString() && auction.AuctioneerId != foundUser.Id)
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
        /// Gets the full name of the owner of the Auction.
        /// </summary>
        /// <returns>The full name of the Auction owner.</returns>
        public string GetOwnerFullName()
        {
            User foundUser = _context.Users
                .AsNoTracking()
                .First(u => u.Id == Auction.AuctioneerId);

            return foundUser.FirstName + " " + foundUser.LastName;
        }

        /// <summary>
        /// Gets the data URL of the main image of the Auction.
        /// </summary>
        /// <returns>The data URL of the main image of the Auction.</returns>
        public string GetImage()
        {
            byte[] data = _context.VehicleImages
                .AsNoTracking()
                .Where(img => img.VehicleId == Auction.VehicleId)
                .First()
                .ImageData;

            string imgDataURL = "data:image;base64," + Convert.ToBase64String(data);
            return imgDataURL;
        }
    }
}
