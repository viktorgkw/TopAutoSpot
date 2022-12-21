using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TopAutoSpot.Data;
using TopAutoSpot.Data.Entities;

namespace TopAutoSpot.Views.AdministratorViews.UsersCRUD
{
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;

        public EditModel(UserManager<User> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [BindProperty]
        public User UserToEdit { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string userId)
        {
            if (User.IsInRole("Administrator"))
            {
                var isIdValid = await UserIdIsValid(userId);

                if (!isIdValid)
                {
                    return RedirectToPage("/NotFound");
                }

                UserToEdit = await _context.Users.FirstAsync(u => u.Id == userId);

                return Page();
            }

            return RedirectToPage("/NotFound");
        }

        public async Task<IActionResult> OnPostAsync()
         {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            UserToEdit.NormalizedUserName = UserToEdit.UserName.ToUpper();
            UserToEdit.NormalizedEmail = UserToEdit.Email.ToUpper();

            _context.Attach(UserToEdit).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await UserIdIsValid(UserToEdit.Id))
                {
                    return RedirectToPage("/Index");
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("/AdministratorViews/ManageUsers");
        }

        private async Task<bool> UserIdIsValid(string userId)
        {
            return await _context.Users.AnyAsync(u => u.Id == userId);
        }
    }
}
