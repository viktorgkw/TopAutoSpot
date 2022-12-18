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
            if (User.IsInRole("Administrator"))
            {
                UserToClose = await _context.Users.FirstAsync(u => u.Id == userId);

                _context.Users.Remove(UserToClose);
                await _context.SaveChangesAsync();

                return Page();
            }

            return RedirectToPage("/NotFound");
        }
    }
}
