using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopAutoSpot.Data;
using TopAutoSpot.Data.Entities;
using TopAutoSpot.Views.Utilities;

namespace TopAutoSpot.Views.Notifications
{
    public class PreviewModel : PageModel
    {
        private ApplicationDbContext _context;

        public PreviewModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Notification Notification { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return RedirectToPage("/NotFound");
            }

            Notification = await NotificationServices.Get(_context, id);

            if (Notification == null)
            {
                return RedirectToPage("/NotFound");
            }

            return Page();
        }

        public async Task<string> GetFromUsername()
        {
            return await NotificationServices.GetFromUsername(_context, Notification.From);
        }
    }
}
