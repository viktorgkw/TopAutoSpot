using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TopAutoSpot.Views.AdministratorViews.ApprovalViews
{
    [Authorize]
    public class RefuseAuctionReasonModel : PageModel
    {
        [BindProperty]
        public string Reason { get; set; }

        [BindProperty]
        public string AuctionId { get; set; }

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

        public IActionResult OnPost()
        {
            return RedirectToPage("/AdministratorViews/ApprovalViews/RefuseAuction",
                new { auctionId = AuctionId, reason = Reason });
        }
    }
}
