namespace TopAutoSpot.Views.AuctionViews
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    /// <summary>
    /// Represents a page model for the "No Vehicles to Auction" page, which is only accessible by authorized users.
    /// </summary>
    [Authorize]
    public class NoVehiclesToAuctionModel : PageModel
    {
        /// <summary>
        /// Called when the "No Vehicles to Auction" page is requested via HTTP GET.
        /// </summary>
        /// <returns>The "No Vehicles to Auction" page.</returns>
        public IActionResult OnGet()
        {
            return Page();
        }
    }
}
