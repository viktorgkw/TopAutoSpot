using Microsoft.EntityFrameworkCore;
using TopAutoSpot.Data;
using TopAutoSpot.Models;

namespace TopAutoSpot.Views.Utilities
{
    public static class UserServices
    {
        public static async Task<User> GetUserById(ApplicationDbContext _context, string userId)
        {
            return await _context.Users.FirstAsync(u => u.Id == userId);
        }

        public static async Task<User> GetUserByName(ApplicationDbContext _context, string username)
        {
            return await _context.Users.FirstAsync(u => u.UserName == username);
        }

        public static async Task<string> GetCurrentUser(ApplicationDbContext _context, string username)
        {
            return _context.Users.First(u => u.UserName == username).Id;
        }

        public static async Task<string> GetVehicleOwner(ApplicationDbContext _context, string vehicleId)
        {
            var Boats = await _context.Boats
                .Where(boat => boat.Id == vehicleId)
                .ToListAsync();

            if (Boats.Count > 0)
            {
                return Boats.First().CreatedBy;
            }

            var Buses = await _context.Buses
                .Where(bus => bus.Id == vehicleId)
                .ToListAsync();

            if (Buses.Count > 0)
            {
                return Buses.First().CreatedBy;
            }

            var Cars = await _context.Cars
                .Where(car => car.Id == vehicleId)
                .ToListAsync();

            if (Cars.Count > 0)
            {
                return Cars.First().CreatedBy;
            }

            var Motorcycles = await _context.Motorcycles
                .Where(motorcycle => motorcycle.Id == vehicleId)
                .ToListAsync();

            if (Motorcycles.Count > 0)
            {
                return Motorcycles.First().CreatedBy;
            }

            var Trailers = await _context.Trailers
                .Where(trailer => trailer.Id == vehicleId)
                .ToListAsync();

            if (Trailers.Count > 0)
            {
                return Trailers.First().CreatedBy;
            }

            var Trucks = await _context.Trucks
                .Where(truck => truck.Id == vehicleId)
                .ToListAsync();

            if (Trucks.Count > 0)
            {
                return Trucks.First().CreatedBy;
            }

            return "";
        }
    }
}
