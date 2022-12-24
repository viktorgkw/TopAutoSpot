using TopAutoSpot.Data;
using TopAutoSpot.Models;

namespace TopAutoSpot.Views.Utilities
{
    public static class UserServices
    {
        public static string GetVehicleIdByTitle(ApplicationDbContext _context, string title)
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

        public static List<string> GetUserVehicles(ApplicationDbContext _context, string userId)
        {
            List<string> result = new List<string>();

            foreach (Car? vehicle in _context.Cars.Where(v => v.CreatedBy == userId))
            {
                result.Add(vehicle.Title);
            }
            foreach (Motorcycle? vehicle in _context.Motorcycles.Where(v => v.CreatedBy == userId))
            {
                result.Add(vehicle.Title);
            }
            foreach (Truck? vehicle in _context.Trucks.Where(v => v.CreatedBy == userId))
            {
                result.Add(vehicle.Title);
            }
            foreach (Boat? vehicle in _context.Boats.Where(v => v.CreatedBy == userId))
            {
                result.Add(vehicle.Title);
            }
            foreach (Bus? vehicle in _context.Buses.Where(v => v.CreatedBy == userId))
            {
                result.Add(vehicle.Title);
            }
            foreach (Trailer? vehicle in _context.Trailers.Where(v => v.CreatedBy == userId))
            {
                result.Add(vehicle.Title);
            }

            return result;
        }

        public static User GetUserById(ApplicationDbContext _context, string userId)
        {
            return _context.Users.First(u => u.Id == userId);
        }

        public static User GetUserByName(ApplicationDbContext _context, string username)
        {
            return _context.Users.First(u => u.UserName == username);
        }

        public static string GetCurrentUser(ApplicationDbContext _context, string username)
        {
            return _context.Users.First(u => u.UserName == username).Id;
        }

        public static string GetVehicleOwner(ApplicationDbContext _context, string vehicleId)
        {
            List<Boat> Boats = _context.Boats
                .Where(boat => boat.Id == vehicleId)
                .ToList();

            if (Boats.Count > 0)
            {
                return Boats.First().CreatedBy;
            }

            List<Bus> Buses = _context.Buses
                .Where(bus => bus.Id == vehicleId)
                .ToList();

            if (Buses.Count > 0)
            {
                return Buses.First().CreatedBy;
            }

            List<Car> Cars = _context.Cars
                .Where(vehicle => vehicle.Id == vehicleId)
                .ToList();

            if (Cars.Count > 0)
            {
                return Cars.First().CreatedBy;
            }

            List<Motorcycle> Motorcycles = _context.Motorcycles
                .Where(motorcycle => motorcycle.Id == vehicleId)
                .ToList();

            if (Motorcycles.Count > 0)
            {
                return Motorcycles.First().CreatedBy;
            }

            List<Trailer> Trailers = _context.Trailers
                .Where(trailer => trailer.Id == vehicleId)
                .ToList();

            if (Trailers.Count > 0)
            {
                return Trailers.First().CreatedBy;
            }

            List<Truck> Trucks = _context.Trucks
                .Where(truck => truck.Id == vehicleId)
                .ToList();

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
