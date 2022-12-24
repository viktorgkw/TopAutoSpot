using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TopAutoSpot.Views.AuctionViews
{
    [Authorize]
    public class NoVehiclesToAuctionModel : PageModel
    {
        public IActionResult OnGet()
        {
            return Page();
        }
    }
}
