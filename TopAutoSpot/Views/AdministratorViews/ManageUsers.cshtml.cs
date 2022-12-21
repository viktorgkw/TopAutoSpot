using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
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

        public async Task<IActionResult> OnGetAsync()
        {
            if (User.IsInRole("Administrator"))
            {
                Users = await _context.Users
                    .OrderBy(u => u.Role)
                    .ToListAsync();

                return Page();
            }

            return RedirectToPage("/NotFound");
        }
    }
}
