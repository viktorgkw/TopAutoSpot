using TopAutoSpot.Data;
using TopAutoSpot.Models;
using TopAutoSpot.Views.Utilities;
using TopAutoSpot.Services.EmailService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace TopAutoSpot.Views.AdministratorViews.UsersCRUD
{
    [Authorize]
    public class EditModel : PageModel
    {
        private ApplicationDbContext _context;
        private IEmailService _emailService;

        public EditModel(ApplicationDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
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

                _emailService.SendEmail(new EmailDto()
                {
                    To = UserToEdit.Email,
                    Subject = DefaultNotificationMessages.ACCOUNT_EDITED_TITLE,
                    Body = DefaultNotificationMessages.ACCOUNT_EDITED_DESCRIPTION
                });
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
