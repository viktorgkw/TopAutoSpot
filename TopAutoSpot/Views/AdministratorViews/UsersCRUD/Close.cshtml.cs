using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopAutoSpot.Data.Entities;
using TopAutoSpot.Data;
using Microsoft.EntityFrameworkCore;
using TopAutoSpot.Data.Entities.Utilities;

namespace TopAutoSpot.Views.AdministratorViews.UsersCRUD
{
    [Authorize]
    public class CloseModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CloseModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public User UserToClose { get; set; }

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
                UserToClose = await _context.Users.FirstAsync(u => u.Id == userId);

                _context.Users.Remove(UserToClose);
                await _context.SaveChangesAsync();

                return Page();
            }
        }
    }
}
