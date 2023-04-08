namespace TopAutoSpot.Views.AdministratorViews.UsersCRUD
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.EntityFrameworkCore;

    using TopAutoSpot.Data;
    using TopAutoSpot.Data.Models;
    using TopAutoSpot.Services.Common;
    using TopAutoSpot.Services.EmailServices;

    /// <summary>
    /// PageModel class for editing a user's account details.
    /// Requires the user to be authenticated with the "Administrator" role.
    /// Allows the administrator to modify a user's account details,
    /// including their username and email, and sends an email notification to the user when the changes are saved.
    /// </summary>
    [Authorize]
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;

        /// <summary>
        /// Constructor for the EditModel class. 
        /// </summary>
        /// <param name="context">The database context for this instance.</param>
        /// <param name="emailService">The email service for sending email notifications.</param>
        public EditModel(ApplicationDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        [BindProperty]
        public User UserToEdit { get; set; } = default!;

        /// <summary>
        /// Handler for the GET request of this page. 
        /// </summary>
        /// <param name="userId">The ID of the user to be edited.</param>
        /// <returns>An IActionResult representing the result of the GET request.</returns>
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

        /// <summary>
        /// Handler for the POST request of this page.
        /// </summary>
        /// <returns>An IActionResult representing the result of the POST request.</returns>
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            UserToEdit.NormalizedUserName = UserToEdit.UserName!.ToUpper();
            UserToEdit.NormalizedEmail = UserToEdit.Email!.ToUpper();

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

        /// <summary>
        /// Checks whether the provided user ID is valid by checking if it exists in the database. 
        /// </summary>
        /// <param name="userId">The ID of the user to check.</param>
        /// <returns>A boolean value indicating whether the user ID is valid or not.</returns>
        private bool UserIdIsValid(string userId)
        {
            return _context.Users.Any(u => u.Id == userId);
        }
    }
}
