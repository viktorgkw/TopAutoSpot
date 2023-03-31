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
        private readonly ApplicationDbContext _context;

        public NotificationRemovedModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet(string id)
        {
            if (id == null)
            {
                return RedirectToPage("/NotFound");
            }

            bool result = NotificationServices.RemoveNotification(_context, id, User.Identity!.Name!);

            if (result == false)
            {
                return RedirectToPage("/NotFound");
            }

            return Page();
        }
    }
}
