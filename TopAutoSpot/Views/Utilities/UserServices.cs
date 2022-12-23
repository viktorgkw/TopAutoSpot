using TopAutoSpot.Data;
using TopAutoSpot.Models;
using Microsoft.EntityFrameworkCore;

namespace TopAutoSpot.Views.Utilities
{
    public static class UserServices
    {
        public static async Task<string> GetVehicleIdByTitle(ApplicationDbContext _context, string title)
        {
            if (_context.Cars.Any(v => v.Title == title))
            {
                return _context.Cars.First(v => v.Title == title).Id;
            }
            else if (_context.Motorcycles.Any(v => v.Title == title))
            {
                return _context.Motorcycles.First(v => v.Title == title).Id;
            }
            else if (_context.Trailers.Any(v => v.Title == title))
            {
                return _context.Trailers.First(v => v.Title == title).Id;
            }
            else if (_context.Trucks.Any(v => v.Title == title))
            {
                return _context.Trucks.First(v => v.Title == title).Id;
            }
            else if (_context.Boats.Any(v => v.Title == title))
            {
                return _context.Boats.First(v => v.Title == title).Id;
            }
            else if (_context.Buses.Any(v => v.Title == title))
            {
                return _context.Buses.First(v => v.Title == title).Id;
            }

            return "";
        }

        public static async Task<List<string>> GetUserVehicles(ApplicationDbContext _context, string userId)
        {
            List<string> result = new List<string>();

            foreach (var vehicle in _context.Cars.Where(v => v.CreatedBy == userId))
            {
                result.Add(vehicle.Title);
            }
            foreach (var vehicle in _context.Motorcycles.Where(v => v.CreatedBy == userId))
            {
                result.Add(vehicle.Title);
            }
            foreach (var vehicle in _context.Trucks.Where(v => v.CreatedBy == userId))
            {
                result.Add(vehicle.Title);
            }
            foreach (var vehicle in _context.Boats.Where(v => v.CreatedBy == userId))
            {
                result.Add(vehicle.Title);
            }
            foreach (var vehicle in _context.Buses.Where(v => v.CreatedBy == userId))
            {
                result.Add(vehicle.Title);
            }
            foreach (var vehicle in _context.Trailers.Where(v => v.CreatedBy == userId))
            {
                result.Add(vehicle.Title);
            }

            return result;
        }

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
                .Where(vehicle => vehicle.Id == vehicleId)
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

        public static string GetAuctionOwner(ApplicationDbContext _context, string auctionId)
        {
            return _context.Auctions.First(a => a.Id == auctionId).AuctioneerId;
        }
    }
}
