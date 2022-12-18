using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TopAutoSpot.Data;
using TopAutoSpot.Data.Entities.Utilities;

namespace TopAutoSpot.Views.AdministratorViews
{
    [Authorize]
    public class ApprovalModel : PageModel
    {
        private ApplicationDbContext _context;

        public ApprovalModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync()
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
                return Page();
            }
        }
    }
}
