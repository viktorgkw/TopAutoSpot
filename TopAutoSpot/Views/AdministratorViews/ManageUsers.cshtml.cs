namespace TopAutoSpot.Views.AdministratorViews
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;

    using TopAutoSpot.Data;
    using TopAutoSpot.Data.Models;

    /// <summary>
    /// The ManageUsersModel is a PageModel that represents the page for managing user accounts.
    /// </summary>
    [Authorize]
    public class ManageUsersModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ManageUsersModel(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// The Users property is a list of User objects that will be displayed on the page.
        /// </summary>
        [BindProperty]
        public List<User> Users { get; set; } = null!;

        /// <summary>
        /// The OnGet method retrieves the list of users and displays them on the page.
        /// </summary>
        /// <returns>Returns the page with the list of users if the user is an Administrator, otherwise redirects to NotFound page.</returns>
        public IActionResult OnGet()
        {
            if (User.IsInRole("Administrator"))
            {
                Users = _context.Users
                    .AsNoTracking()
                    .OrderBy(u => u.Role)
                    .ToList();

                return Page();
            }

            return RedirectToPage("/NotFound");
        }
    }
}
