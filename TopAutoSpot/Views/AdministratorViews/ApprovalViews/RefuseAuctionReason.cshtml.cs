namespace TopAutoSpot.Views.AdministratorViews.ApprovalViews
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    /// <summary>
    /// Page model class for handling the refusal of an auction with a reason for an administrator.
    /// </summary>
    [Authorize]
    public class RefuseAuctionReasonModel : PageModel
    {
        /// <summary>
        /// The reason for the refusal of the auction.
        /// </summary>
        [BindProperty]
        public string Reason { get; set; } = null!;

        /// <summary>
        /// The ID of the auction being refused.
        /// </summary>
        [BindProperty]
        public string AuctionId { get; set; } = null!;

        /// <summary>
        /// Handles the GET request and retrieves the auction ID.
        /// </summary>
        /// <param name="auctionId">The ID of the auction being refused.</param>
        /// <returns>The page.</returns>
        public IActionResult OnGet(string auctionId)
        {
            if (User.IsInRole("Administrator"))
            {
                if (auctionId == null)
                {
                    return RedirectToPage("/NotFound");
                }

                AuctionId = auctionId;

                return Page();
            }

            return RedirectToPage("/NotFound");
        }

        /// <summary>
        /// Handles the POST request and redirects to the page for refusing an auction.
        /// </summary>
        /// <returns>The page for refusing an auction.</returns>
        public IActionResult OnPost()
        {
            return RedirectToPage("/AdministratorViews/ApprovalViews/RefuseAuction",
                new { auctionId = AuctionId, reason = Reason });
        }
    }
}
