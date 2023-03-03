using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TopAutoSpot.Data;
using TopAutoSpot.Data.Models;
using TopAutoSpot.Services.EmailService;
using TopAutoSpot.Views.Utilities;

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

        public IActionResult OnGet(string userId)
        {
            if (User.IsInRole("Administrator"))
            {
                bool isIdValid = UserIdIsValid(userId);

                if (!isIdValid)
                {
                    return RedirectToPage("/NotFound");
                }

                UserToEdit = _context.Users.First(u => u.Id == userId);

                return Page();
            }

            return RedirectToPage("/NotFound");
        }

        public IActionResult OnPost()
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
                _context.SaveChanges();

                _emailService.SendEmail(new EmailDto()
                {
                    To = UserToEdit.Email,
                    Subject = DefaultNotificationMessages.ACCOUNT_EDITED_TITLE,
                    Body = DefaultNotificationMessages.ACCOUNT_EDITED_DESCRIPTION
                });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserIdIsValid(UserToEdit.Id))
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

        private bool UserIdIsValid(string userId)
        {
            return _context.Users.Any(u => u.Id == userId);
        }
    }
}
