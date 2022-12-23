using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TopAutoSpot.Views.AuctionViews
{
    [Authorize]
    public class JoinModel : PageModel
    {
        // Validate that the auction is not in Status != Active
    }
}
