using TopAutoSpot.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace TopAutoSpot.Areas.Identity.Pages.Account
{
    public class ConfirmEmailModel : PageModel
    {
        private ApplicationDbContext _context;
        public ConfirmEmailModel(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> OnGetAsync(string id)
        {
            var foundUser = await _context.Users.FirstAsync(u => u.Id == id);

            foundUser.EmailConfirmed = true;
            await _context.SaveChangesAsync();

            return RedirectToPage("/Account/Login", new { area = "Identity" });
        }
    }
}
