using TopAutoSpot.Data;
using TopAutoSpot.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

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
            if (User.IsInRole("Administrator"))
            {
                PreviewedUser = await _context.Users.FirstAsync(u => u.Id == userId);
                return Page();
            }

            return RedirectToPage("/NotFound");
        }
    }
}
