namespace TopAutoSpot.Views.AdministratorViews.ApprovalViews
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
    /// Page model class to handle refusing a vehicle listing.
    /// </summary>
    [Authorize]
    public class RefuseVehicleModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;

        /// <summary>
        /// Constructor for RefuseVehicleModel.
        /// </summary>
        /// <param name="context">The application database context.</param>
        /// <param name="emailService">The email service.</param>
        public RefuseVehicleModel(ApplicationDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        /// <summary>
        /// Gets or sets the vehicle ID to refuse.
        /// </summary>
        [BindProperty]
        public string VehicleId { get; set; } = null!;

        /// <summary>
        /// Handles GET requests to refuse a vehicle listing.
        /// </summary>
        /// <param name="vehicleId">The ID of the vehicle to refuse.</param>
        /// <param name="reason">The reason for refusing the vehicle.</param>
        /// <returns>The appropriate IActionResult based on the success of the refuse operation.</returns>
        public IActionResult OnGet(string vehicleId, string reason)
        {
            if (User.IsInRole("Administrator"))
            {
                bool refuseResult = VehicleRefused(vehicleId);

                if (refuseResult)
                {
                    VehicleId = vehicleId;

                    string ownerId = UserServices.GetVehicleOwner(_context, VehicleId);
                    User owner = UserServices.GetUserById(_context, ownerId);

                    string currentUserId = UserServices.GetCurrentUser(_context, User.Identity!.Name!);

                    if (ownerId == "" || ownerId == null || currentUserId == null || currentUserId == "")
                    {
                        return RedirectToPage("/NotFound");

                    }

                    bool sendResult = NotificationServices.Send(_context,
                        currentUserId,
                        ownerId,
                        DefaultNotificationMessages.LISTING_REFUSED_TITLE,
                        reason);

                    if (sendResult == false)
                    {
                        return RedirectToPage("/Error");
                    }

                    _emailService.SendEmail(new EmailDto()
                    {
                        To = owner.Email!,
                        Subject = DefaultNotificationMessages.LISTING_REFUSED_TITLE,
                        Body = reason
                    });

                    return Page();
                }
                else
                {
                    return RedirectToPage("/UnknownError");
                }
            }

            return RedirectToPage("/NotFound");
        }

        /// <summary>
        /// Refuses a vehicle listing by setting its status to closed.
        /// </summary>
        /// <param name="vehicleId">The ID of the vehicle to refuse.</param>
        /// <returns>True if the vehicle was successfully refused, false otherwise.</returns>
        public bool VehicleRefused(string vehicleId)
        {
            if (_context.Cars.FirstOrDefault(c => c.Id == vehicleId) != null)
            {
                _context.Cars
                    .First(c => c.Id == vehicleId).Status = ListingStatusTypes.Closed.ToString();
                _context.SaveChanges();

                return true;
            }
            else if (_context.Motorcycles.FirstOrDefault(m => m.Id == vehicleId) != null)
            {
                _context.Motorcycles
                    .First(m => m.Id == vehicleId).Status = ListingStatusTypes.Closed.ToString();
                _context.SaveChanges();

                return true;
            }
            else if (_context.Trucks.FirstOrDefault(t => t.Id == vehicleId) != null)
            {
                _context.Trucks
                    .First(t => t.Id == vehicleId).Status = ListingStatusTypes.Closed.ToString();
                _context.SaveChanges();

                return true;
            }
            else if (_context.Trailers.FirstOrDefault(t => t.Id == vehicleId) != null)
            {
                _context.Trailers
                    .First(t => t.Id == vehicleId).Status = ListingStatusTypes.Closed.ToString();
                _context.SaveChanges();

                return true;
            }
            else if (_context.Buses.FirstOrDefault(b => b.Id == vehicleId) != null)
            {
                _context.Buses
                    .First(b => b.Id == vehicleId).Status = ListingStatusTypes.Closed.ToString();
                _context.SaveChanges();

                return true;
            }
            else if (_context.Boats.FirstOrDefault(b => b.Id == vehicleId) != null)
            {
                _context.Boats.First(b => b.Id == vehicleId).Status = ListingStatusTypes.Closed.ToString();
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
