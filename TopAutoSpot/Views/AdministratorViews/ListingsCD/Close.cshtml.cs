using TopAutoSpot.Data;
using TopAutoSpot.Models;
using TopAutoSpot.Models.Utilities;
using TopAutoSpot.Views.Utilities;
using TopAutoSpot.Services.EmailService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;

namespace TopAutoSpot.Views.AdministratorViews.ListingsCD
{
    [Authorize]
    public class CloseModel : PageModel
    {
        private ApplicationDbContext _context;
        private IEmailService _emailService;

        public CloseModel(ApplicationDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (User.IsInRole("Administrator"))
            {
                var closeResult = await CloseListing(id);

                if (!closeResult)
                {
                    return RedirectToPage("/UnknownError");
                }

                var ownerId = await UserServices.GetVehicleOwner(_context, id);
                var owner = await UserServices.GetUserById(_context, ownerId);

                var currentUserId = await UserServices.GetCurrentUser(_context, User.Identity.Name);

                var sendResult = await NotificationServices.Send(_context,
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
                    To = owner.Email,
                    Subject = DefaultNotificationMessages.LISTING_CLOSED_TITLE,
                    Body = DefaultNotificationMessages.LISTING_CLOSED_DESCRIPTION
                });

                return Page();
            }

            return RedirectToPage("/NotFound");
        }

        private async Task<bool> CloseListing(string id)
        {
            if (_context.Cars.FirstOrDefault(c => c.Id == id) != null)
            {
                _context.Cars
                    .FirstOrDefault(c => c.Id == id).Status = StatusTypes.Closed.ToString();
                await _context.SaveChangesAsync();

                return true;
            }
            else if (_context.Motorcycles.FirstOrDefault(m => m.Id == id) != null)
            {
                _context.Motorcycles
                    .FirstOrDefault(m => m.Id == id).Status = StatusTypes.Closed.ToString();
                await _context.SaveChangesAsync();

                return true;
            }
            else if (_context.Trucks.FirstOrDefault(t => t.Id == id) != null)
            {
                _context.Trucks
                    .FirstOrDefault(t => t.Id == id).Status = StatusTypes.Closed.ToString();
                await _context.SaveChangesAsync();

                return true;
            }
            else if (_context.Trailers.FirstOrDefault(t => t.Id == id) != null)
            {
                _context.Trailers
                    .FirstOrDefault(t => t.Id == id).Status = StatusTypes.Closed.ToString();
                await _context.SaveChangesAsync();

                return true;
            }
            else if (_context.Buses.FirstOrDefault(b => b.Id == id) != null)
            {
                _context.Buses
                    .FirstOrDefault(b => b.Id == id).Status = StatusTypes.Closed.ToString();
                await _context.SaveChangesAsync();

                return true;
            }
            else if (_context.Boats.FirstOrDefault(b => b.Id == id) != null)
            {
                _context.Boats.FirstOrDefault(b => b.Id == id).Status = StatusTypes.Closed.ToString();
                await _context.SaveChangesAsync();

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
