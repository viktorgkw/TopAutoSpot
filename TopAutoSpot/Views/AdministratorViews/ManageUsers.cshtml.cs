using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopAutoSpot.Data;
using TopAutoSpot.Models;

namespace TopAutoSpot.Views.AdministratorViews
{
    [Authorize]
    public class ManageUsersModel : PageModel
    {
        private ApplicationDbContext _context;

        public ManageUsersModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public List<User> Users { get; set; }

        public IActionResult OnGet()
        {
            if (User.IsInRole("Administrator"))
            {
                Users = _context.Users
                    .OrderBy(u => u.Role)
                    .ToList();

                return Page();
            }

            return RedirectToPage("/NotFound");
        }
    }
}
