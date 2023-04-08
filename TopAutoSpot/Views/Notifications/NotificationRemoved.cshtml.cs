namespace TopAutoSpot.Views.Notifications
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    using TopAutoSpot.Data;
    using TopAutoSpot.Services.Utilities;

    /// <summary>
    /// This page model represents the functionality for removing a notification from the database.
    /// </summary>
    [Authorize]
    public class NotificationRemovedModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Constructor that initializes the ApplicationDbContext instance.
        /// </summary>
        /// <param name="context">ApplicationDbContext instance</param>
        public NotificationRemovedModel(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Removes a notification from the database using the specified id.
        /// </summary>
        /// <param name="id">Id of the notification to be removed</param>
        /// <returns>Redirects to the Page</returns>
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
