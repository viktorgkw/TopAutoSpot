using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TopAutoSpot.Data;
using TopAutoSpot.Data.Models;

namespace TopAutoSpot.Views.AdministratorViews
{
    [Authorize]
    public class ManageUsersModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public ManageUsersModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public List<User> Users { get; set; } = null!;

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
