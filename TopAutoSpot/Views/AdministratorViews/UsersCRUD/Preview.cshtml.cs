using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TopAutoSpot.Data;
using TopAutoSpot.Data.Entities;
using TopAutoSpot.Data.Entities.Utilities;

namespace TopAutoSpot.Views.AdministratorViews.UsersCRUD
{
    [Authorize]
    public class PreviewModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public PreviewModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public User PreviewedUser { get; set; }

        public async Task<IActionResult> OnGetAsync(string userId)
        {
            var foundUser = await _context.Users.FirstAsync(u => u.UserName == User.Identity.Name);

            if (foundUser == null)
            {
                return RedirectToPage("/Index");
            }
            else if (foundUser.Role != RoleTypes.Administrator.ToString())
            {
                return RedirectToPage("/NotFound");
            }
            else
            {
                PreviewedUser = await _context.Users.FirstAsync(u => u.Id == userId);
                return Page();
            }
        }
    }
}
