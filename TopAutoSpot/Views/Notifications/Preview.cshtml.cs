namespace TopAutoSpot.Views.Notifications
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    using TopAutoSpot.Data;
    using TopAutoSpot.Data.Models;
    using TopAutoSpot.Services.Utilities;

    /// <summary>
    /// This page model represents the preview page of a notification.
    /// </summary>
    [Authorize]
    public class PreviewModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public PreviewModel(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets or sets the notification that is being previewed.
        /// </summary>
        [BindProperty]
        public Notification Notification { get; set; } = null!;

        /// <summary>
        /// Handles the OnGet event of the preview page.
        /// </summary>
        /// <param name="id">The ID of the notification to preview.</param>
        /// <returns>Returns a IActionResult representing the page.</returns>
        public IActionResult OnGet(string id)
        {
            if (id == null)
            {
                return RedirectToPage("/NotFound");
            }

            Notification = NotificationServices.Get(_context, id, User.Identity!.Name!)!;

            if (Notification == null)
            {
                return RedirectToPage("/NotFound");
            }

            return Page();
        }

        /// <summary>
        /// Gets the username of the sender of the notification.
        /// </summary>
        /// <returns>Returns a string representing the username of the sender.</returns>
        public string? GetFromUsername()
        {
            return NotificationServices.GetFromUsername(_context, Notification.From);
        }
    }
}
