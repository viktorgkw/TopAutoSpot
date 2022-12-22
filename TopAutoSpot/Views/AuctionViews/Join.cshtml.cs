using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TopAutoSpot.Views.AuctionViews
{
    public class JoinModel : PageModel
    {
        [BindProperty]
        public string Id { get; set; }
        public void OnGet(string id)
        {
            Id = id;
        }
    }
}
