namespace TopAutoSpot.Views.AdministratorViews.ListingsCD
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    using TopAutoSpot.Data;
    using TopAutoSpot.Data.Models;
    using TopAutoSpot.Data.Models.Enums;
    using TopAutoSpot.Services.Common;
    using TopAutoSpot.Services.EmailServices;
    using TopAutoSpot.Services.Utilities;

    /// <summary>
    /// Page model for closing a listing. This page can only be accessed by users in the "Administrator" role.
    /// </summary>
    [Authorize]
    public class CloseModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CloseModel"/> class.
        /// </summary>
        /// <param name="context">The application database context.</param>
        /// <param name="emailService">The email service.</param>
        public CloseModel(ApplicationDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        /// <summary>
        /// Handles HTTP GET requests for closing a listing.
        /// </summary>
        /// <param name="id">The ID of the listing to close.</param>
        /// <returns>Returns a redirect to the "/UnknownError" page if an error occurs during the closing process,
        /// or a redirect to the "/NotFound" page if the user is not in the "Administrator" role.</returns>
        public IActionResult OnGet(string id)
        {
            if (User.IsInRole("Administrator"))
            {
                bool closeResult = CloseListing(id);

                if (!closeResult)
                {
                    return RedirectToPage("/UnknownError");
                }

                string ownerId = UserServices.GetVehicleOwner(_context, id);
                User owner = UserServices.GetUserById(_context, ownerId);

                string currentUserId = UserServices.GetCurrentUser(_context, User.Identity!.Name!);

                bool sendResult = NotificationServices.Send(_context,
                    currentUserId,
                    ownerId,
                    DefaultNotificationMessages.LISTING_CLOSED_TITLE,
                    DefaultNotificationMessages.LISTING_CLOSED_DESCRIPTION);

                if (!sendResult)
                {
                    return RedirectToPage("/UnknownError");
                }

                _emailService.SendEmail(new EmailDto()
                {
                    To = owner.Email!,
                    Subject = DefaultNotificationMessages.LISTING_CLOSED_TITLE,
                    Body = DefaultNotificationMessages.LISTING_CLOSED_DESCRIPTION
                });

                return Page();
            }

            return RedirectToPage("/NotFound");
        }

        /// <summary>
        /// Closes the listing with the specified ID.
        /// </summary>
        /// <param name="id">The ID of the listing to close.</param>
        /// <returns>Returns true if the listing was successfully closed, or false if the listing was not found.</returns>
        private bool CloseListing(string id)
        {
            if (_context.Cars.FirstOrDefault(c => c.Id == id) != null)
            {
                _context.Cars
                    .First(c => c.Id == id).Status = ListingStatusTypes.Closed.ToString();
                _context.SaveChanges();

                return true;
            }
            else if (_context.Motorcycles.FirstOrDefault(m => m.Id == id) != null)
            {
                _context.Motorcycles
                    .First(m => m.Id == id).Status = ListingStatusTypes.Closed.ToString();
                _context.SaveChanges();

                return true;
            }
            else if (_context.Trucks.FirstOrDefault(t => t.Id == id) != null)
            {
                _context.Trucks
                    .First(t => t.Id == id).Status = ListingStatusTypes.Closed.ToString();
                _context.SaveChanges();

                return true;
            }
            else if (_context.Trailers.FirstOrDefault(t => t.Id == id) != null)
            {
                _context.Trailers
                    .First(t => t.Id == id).Status = ListingStatusTypes.Closed.ToString();
                _context.SaveChanges();

                return true;
            }
            else if (_context.Buses.FirstOrDefault(b => b.Id == id) != null)
            {
                _context.Buses
                    .First(b => b.Id == id).Status = ListingStatusTypes.Closed.ToString();
                _context.SaveChanges();

                return true;
            }
            else if (_context.Boats.FirstOrDefault(b => b.Id == id) != null)
            {
                _context.Boats.First(b => b.Id == id).Status = ListingStatusTypes.Closed.ToString();
                _context.SaveChanges();

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
