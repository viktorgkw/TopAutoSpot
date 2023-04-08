namespace TopAutoSpot.Views.AdministratorViews.UsersCRUD
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;

    using TopAutoSpot.Data;
    using TopAutoSpot.Data.Models;

    /// <summary>
    /// Represents a Razor PageModel class that displays a preview of a user profile and requires administrator authorization.
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
        /// The user whose profile is being previewed.
        /// </summary>
        [BindProperty]
        public User PreviewedUser { get; set; } = null!;

        /// <summary>
        /// Handles GET requests and initializes the PreviewedUser property if the user is an administrator.
        /// </summary>
        /// <param name="userId">The ID of the user whose profile is being previewed.</param>
        /// <returns>The page to be displayed or a redirect to the NotFound page.</returns>
        public IActionResult OnGet(string userId)
        {
            if (User.IsInRole("Administrator"))
            {
                PreviewedUser = _context.Users
                    .AsNoTracking()
                    .First(u => u.Id == userId);
                return Page();
            }

            return RedirectToPage("/NotFound");
        }
    }
}
