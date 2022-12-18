using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TopAutoSpot.Data;
using TopAutoSpot.Data.Entities;

namespace TopAutoSpot.Views.Notifications
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private ApplicationDbContext _context;
        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public List<Notification> Notifications { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var currentUser = await _context.Users.FirstAsync(u => u.UserName == User.Identity.Name);

            Notifications = await _context.Notifications
                .Where(n => n.To == currentUser.Id)
                .ToListAsync();

            return Page();
        }
    }
}
