using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopAutoSpot.Data;
using TopAutoSpot.Data.Models;
using TopAutoSpot.Data.Models.Enums;
using TopAutoSpot.Services.Common;
using TopAutoSpot.Services.EmailServices;
using TopAutoSpot.Services.Utilities;

namespace TopAutoSpot.Views.AdministratorViews.ListingsCD
{
    [Authorize]
    public class CloseModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;

        public CloseModel(ApplicationDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

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
