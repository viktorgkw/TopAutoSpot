using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TopAutoSpot.Data;
using TopAutoSpot.Data.Models;

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
            User foundUser = await _context.Users.FirstAsync(u => u.Id == id);

            foundUser.EmailConfirmed = true;
            await _context.SaveChangesAsync();

            return RedirectToPage("/Account/Login", new { area = "Identity" });
        }
    }
}
