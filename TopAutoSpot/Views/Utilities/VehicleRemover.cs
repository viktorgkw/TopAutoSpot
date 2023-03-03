using TopAutoSpot.Data;
using TopAutoSpot.Models;

namespace TopAutoSpot.Views.Utilities
{
    public class VehicleRemover
    {
        private ApplicationDbContext _context;
        public VehicleRemover(ApplicationDbContext db)
        {
            _context = db;
        }

        public Task RemoveAllUserAuctions(string id)
        {
            IQueryable<Auction> auctions = _context.Auctions
                .Where(a => a.AuctioneerId == id);

            foreach (Auction? auction in auctions)
            {
                _context.Auctions.Remove(auction);
                _context.SaveChangesAsync();
            }

            return Task.FromResult(0);
        }

        public Task RemoveAllUserVehicles(string id)
        {
            List<Car> cars = _context.Cars
                .Where(v => v.CreatedBy == id)
                .ToList();

            foreach (Car? car in cars)
            {
                if (car.CreatedBy == id)
                {
                    _context.Cars.Remove(car);
                    _context.SaveChangesAsync();
                }
            }

            List<Motorcycle> motorcycles = _context.Motorcycles
                .Where(v => v.CreatedBy == id)
                .ToList();

            foreach (Motorcycle? moto in motorcycles)
            {
                if (moto.CreatedBy == id)
                {
                    _context.Motorcycles.Remove(moto);
                    _context.SaveChangesAsync();
                }
            }

            List<Truck> trucks = _context.Trucks
                .Where(v => v.CreatedBy == id)
                .ToList();

            foreach (Truck? truck in trucks)
            {
                if (truck.CreatedBy == id)
                {
                    _context.Trucks.Remove(truck);
                    _context.SaveChangesAsync();
                }
            }

            List<Trailer> trailers = _context.Trailers
                .Where(v => v.CreatedBy == id)
                .ToList();

            foreach (Trailer? trailer in trailers)
            {
                if (trailer.CreatedBy == id)
                {
                    _context.Trailers.Remove(trailer);
                    _context.SaveChangesAsync();
                }
            }

            List<Boat> boats = _context.Boats
                .Where(v => v.CreatedBy == id)
                .ToList();

            foreach (Boat? boat in boats)
            {
                if (boat.CreatedBy == id)
                {
                    _context.Boats.Remove(boat);
                    _context.SaveChangesAsync();
                }
            }

            List<Bus> buses = _context.Buses
                .Where(v => v.CreatedBy == id)
                .ToList();

            foreach (Bus? bus in buses)
            {
                if (bus.CreatedBy == id)
                {
                    _context.Buses.Remove(bus);
                    _context.SaveChangesAsync();
                }
            }

            return Task.FromResult(0);
        }
    }
}
