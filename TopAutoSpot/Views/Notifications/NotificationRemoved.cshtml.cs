using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopAutoSpot.Data;
using TopAutoSpot.Views.Utilities;

namespace TopAutoSpot.Views.Notifications
{
    [Authorize]
    public class NotificationRemovedModel : PageModel
    {
        private ApplicationDbContext _context;
        public NotificationRemovedModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return RedirectToPage("/NotFound");
            }

            var result = await NotificationServices.RemoveNotification(_context, id);

            if (result == false)
            {
                return RedirectToPage("/NotFound");
            }

            return Page();
        }
    }
}
