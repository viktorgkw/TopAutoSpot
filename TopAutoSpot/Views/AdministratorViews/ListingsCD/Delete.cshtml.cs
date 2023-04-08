namespace TopAutoSpot.Views.AdministratorViews.ListingsCD
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    using TopAutoSpot.Data;
    using TopAutoSpot.Data.Models;
    using TopAutoSpot.Services.Common;
    using TopAutoSpot.Services.EmailServices;
    using TopAutoSpot.Services.Utilities;

    /// <summary>
    /// This class handles the deletion of vehicles from the database, including sending a notification email to the owner of the vehicle.
    /// </summary>
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;

        /// <summary>
        /// Constructor for the DeleteModel class.
        /// </summary>
        /// <param name="context">The application database context.</param>
        /// <param name="emailService">The email service to use for sending notification emails.</param>
        public DeleteModel(ApplicationDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        /// <summary>
        /// The ID of the vehicle to be deleted, bound to a property for easy access.
        /// </summary>
        [BindProperty]
        public string VehicleId { get; set; } = null!;

        /// <summary>
        /// Handles HTTP GET requests to the page, deleting the specified vehicle and sending a notification email to the owner.
        /// </summary>
        /// <param name="id">The ID of the vehicle to delete.</param>
        /// <returns>An IActionResult representing the result of the operation.</returns>
        public IActionResult OnGet(string id)
        {
            if (User.IsInRole("Administrator"))
            {
                VehicleId = id;

                string ownerId = UserServices.GetVehicleOwner(_context, VehicleId);
                User owner = UserServices.GetUserById(_context, ownerId);

                string currentUserId = UserServices.GetCurrentUser(_context, User.Identity!.Name!);

                bool sendResult = NotificationServices.Send(_context,
                    currentUserId,
                    ownerId,
                    DefaultNotificationMessages.LISTING_DELETED_TITLE,
                    DefaultNotificationMessages.LISTING_DELETED_DESCRIPTION);

                if (!sendResult)
                {
                    return RedirectToPage("/UnknownError");
                }

                bool deleteResult = DeleteVehicle(id);

                if (!deleteResult)
                {
                    return RedirectToPage("/UnknownError");
                }

                _emailService.SendEmail(new EmailDto()
                {
                    To = owner.Email!,
                    Subject = DefaultNotificationMessages.LISTING_DELETED_TITLE,
                    Body = DefaultNotificationMessages.LISTING_DELETED_DESCRIPTION
                });

                return Page();
            }

            return RedirectToPage("/NotFound");
        }

        /// <summary>
        /// Deletes the specified vehicle from the database.
        /// </summary>
        /// <param name="id">The ID of the vehicle to delete.</param>
        /// <returns>True if the deletion was successful, false otherwise.</returns>
        private bool DeleteVehicle(string id)
        {
            if (_context.Cars.FirstOrDefault(c => c.Id == id) != null)
            {
                Car foundVehicle = _context.Cars.First(v => v.Id == id);
                _context.Cars.Remove(foundVehicle);
                _context.SaveChanges();

                return true;
            }
            else if (_context.Motorcycles.FirstOrDefault(m => m.Id == id) != null)
            {
                Motorcycle foundVehicle = _context.Motorcycles.First(v => v.Id == id);
                _context.Motorcycles.Remove(foundVehicle);
                _context.SaveChanges();

                return true;
            }
            else if (_context.Trucks.FirstOrDefault(t => t.Id == id) != null)
            {
                Truck foundVehicle = _context.Trucks.First(v => v.Id == id);
                _context.Trucks.Remove(foundVehicle);
                _context.SaveChanges();

                return true;
            }
            else if (_context.Trailers.FirstOrDefault(t => t.Id == id) != null)
            {
                Trailer foundVehicle = _context.Trailers.First(v => v.Id == id);
                _context.Trailers.Remove(foundVehicle);
                _context.SaveChanges();

                return true;
            }
            else if (_context.Buses.FirstOrDefault(b => b.Id == id) != null)
            {
                Bus foundVehicle = _context.Buses.First(v => v.Id == id);
                _context.Buses.Remove(foundVehicle);
                _context.SaveChanges();

                return true;
            }
            else if (_context.Boats.FirstOrDefault(b => b.Id == id) != null)
            {
                Boat foundVehicle = _context.Boats.First(v => v.Id == id);
                _context.Boats.Remove(foundVehicle);
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
