using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopAutoSpot.Data;
using TopAutoSpot.Data.Models;
using TopAutoSpot.Services.Common;
using TopAutoSpot.Services.EmailServices;
using TopAutoSpot.Services.Utilities;

namespace TopAutoSpot.Views.AdministratorViews.ListingsCD
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        private readonly IEmailService _emailService;

        public DeleteModel(ApplicationDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        [BindProperty]
        public string VehicleId { get; set; } = null!;

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
