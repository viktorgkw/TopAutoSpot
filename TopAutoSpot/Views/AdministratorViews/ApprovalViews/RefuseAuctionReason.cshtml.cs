using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;

namespace TopAutoSpot.Views.AdministratorViews.ApprovalViews
{
    [Authorize]
    public class RefuseAuctionReasonModel : PageModel
    {
        [BindProperty]
        public string Reason { get; set; }

        [BindProperty]
        public string AuctionId { get; set; }

        public async Task<IActionResult> OnGetAsync(string auctionId)
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

        public async Task<IActionResult> OnPostAsync()
        {
            return RedirectToPage("/AdministratorViews/ApprovalViews/RefuseAuction", new { auctionId = AuctionId, reason = Reason });
        }
    }
}
