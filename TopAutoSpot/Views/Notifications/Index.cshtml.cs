namespace TopAutoSpot.Views.Notifications
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    using TopAutoSpot.Data;
    using TopAutoSpot.Data.Models;
    using TopAutoSpot.Services.Utilities;

    ///<summary>
    ///PageModel for the Notifications Index page.
    ///</summary>
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Property for the list of notifications to be displayed on the page.
        /// </summary>
        [BindProperty]
        public List<Notification> Notifications { get; set; } = null!;

        /// <summary>
        /// Displays the notifications on the page for the logged in user.
        /// </summary>
        /// <returns>The page result</returns>
        public IActionResult OnGet()
        {
            User currentUser = UserServices.GetUserByName(_context, User.Identity!.Name!);

            Notifications = NotificationServices.GetAll(_context, currentUser.Id);

            return Page();
        }

        /// <summary>
        /// Gets the sender of the notification.
        /// </summary>
        /// <param name="senderId">The id of the sender</param>
        /// <returns>The username of the sender</returns>
        public string GetNotificationSender(string senderId)
        {
            return NotificationServices.GetFromUsername(_context, senderId)!;
        }
    }
}
