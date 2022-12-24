using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopAutoSpot.Data;
using TopAutoSpot.Models;

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

        public IActionResult OnGet(string userId)
        {
            if (User.IsInRole("Administrator"))
            {
                PreviewedUser = _context.Users.First(u => u.Id == userId);
                return Page();
            }

            return RedirectToPage("/NotFound");
        }
    }
}
