using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TopAutoSpot.Data;
using TopAutoSpot.Data.Entities.Utilities;

namespace TopAutoSpot.Views.AdministratorViews.ListingsCD
{
    [Authorize]
    public class DeleteModel : PageModel
    {
        private ApplicationDbContext _context;

        public DeleteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string VehicleId { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (User.IsInRole("Administrator"))
            {
                var result = await DeleteVehicle(id);

                if (!result)
                {
                    return RedirectToPage("/NotFound");
                }

                VehicleId = id;
                return Page();
            }

            return RedirectToPage("/NotFound");
        }

        private async Task<bool> DeleteVehicle(string id)
        {
            if (_context.Cars.FirstOrDefault(c => c.Id == id) != null)
            {
                var foundVehicle = await _context.Cars.FirstAsync(v => v.Id == id);
                _context.Cars.Remove(foundVehicle);
                await _context.SaveChangesAsync();

                return true;
            }
            else if (_context.Motorcycles.FirstOrDefault(m => m.Id == id) != null)
            {
                var foundVehicle = await _context.Motorcycles.FirstAsync(v => v.Id == id);
                _context.Motorcycles.Remove(foundVehicle);
                await _context.SaveChangesAsync();

                return true;
            }
            else if (_context.Trucks.FirstOrDefault(t => t.Id == id) != null)
            {
                var foundVehicle = await _context.Trucks.FirstAsync(v => v.Id == id);
                _context.Trucks.Remove(foundVehicle);
                await _context.SaveChangesAsync();

                return true;
            }
            else if (_context.Trailers.FirstOrDefault(t => t.Id == id) != null)
            {
                var foundVehicle = await _context.Trailers.FirstAsync(v => v.Id == id);
                _context.Trailers.Remove(foundVehicle);
                await _context.SaveChangesAsync();

                return true;
            }
            else if (_context.Buses.FirstOrDefault(b => b.Id == id) != null)
            {
                var foundVehicle = await _context.Buses.FirstAsync(v => v.Id == id);
                _context.Buses.Remove(foundVehicle);
                await _context.SaveChangesAsync();

                return true;
            }
            else if (_context.Boats.FirstOrDefault(b => b.Id == id) != null)
            {
                var foundVehicle = await _context.Boats.FirstAsync(v => v.Id == id);
                _context.Boats.Remove(foundVehicle);
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
