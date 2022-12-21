using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopAutoSpot.Data;
using TopAutoSpot.Models;
using TopAutoSpot.Views.Utilities;

namespace TopAutoSpot.Views.Notifications
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private ApplicationDbContext _context;
        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public List<Notification> Notifications { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var currentUser = await UserServices.GetUserByName(_context, User.Identity.Name);

            Notifications = await NotificationServices.GetAll(_context, currentUser.Id);

            return Page();
        }

        public async Task<string> GetNotificationSender(string senderId)
        {
            return await NotificationServices.GetFromUsername(_context, senderId);
        }
    }
}
