using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TopAutoSpot.Data;
using TopAutoSpot.Data.Entities.Utilities;

namespace TopAutoSpot.Views.AdministratorViews.ListingsCD
{
    [Authorize]
    public class CloseModel : PageModel
    {
        private ApplicationDbContext _context;

        public CloseModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (User.IsInRole("Administrator"))
            {
                var result = await CloseListing(id);

                if (!result)
                {
                    return RedirectToPage("/NotFound");
                }

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