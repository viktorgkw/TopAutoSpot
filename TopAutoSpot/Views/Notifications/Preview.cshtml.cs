using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopAutoSpot.Data;
using TopAutoSpot.Models;
using TopAutoSpot.Views.Utilities;

namespace TopAutoSpot.Views.Notifications
{
    [Authorize]
    public class PreviewModel : PageModel
    {
        private ApplicationDbContext _context;

        public PreviewModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Notification Notification { get; set; }

        public IActionResult OnGet(string id)
        {
            if (id == null)
            {
                return RedirectToPage("/NotFound");
            }

            Notification = NotificationServices.Get(_context, id, User.Identity.Name);

            if (Notification == null)
            {
                return RedirectToPage("/NotFound");
            }

            return Page();
        }

        public string GetFromUsername()
        {
            return NotificationServices.GetFromUsername(_context, Notification.From);
        }
    }
}
