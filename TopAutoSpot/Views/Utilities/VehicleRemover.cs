using TopAutoSpot.Data;

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
            IQueryable<Models.Auction> auctions = _context.Auctions.Where(a => a.AuctioneerId == id);

            foreach (Models.Auction? auction in auctions)
            {
                _context.Auctions.Remove(auction);
                _context.SaveChangesAsync();
            }

            return Task.FromResult(0);
        }

        public Task RemoveAllUserVehicles(string id)
        {
            List<Models.Car> cars = _context.Cars.Where(v => v.CreatedBy == id).ToList();
            foreach (Models.Car? car in cars)
            {
                if (car.CreatedBy == id)
                {
                    _context.Cars.Remove(car);
                    _context.SaveChangesAsync();
                }
            }

            List<Models.Motorcycle> motorcycles = _context.Motorcycles.Where(v => v.CreatedBy == id).ToList();
            foreach (Models.Motorcycle? moto in motorcycles)
            {
                if (moto.CreatedBy == id)
                {
                    _context.Motorcycles.Remove(moto);
                    _context.SaveChangesAsync();
                }
            }

            List<Models.Truck> trucks = _context.Trucks.Where(v => v.CreatedBy == id).ToList();
            foreach (Models.Truck? truck in trucks)
            {
                if (truck.CreatedBy == id)
                {
                    _context.Trucks.Remove(truck);
                    _context.SaveChangesAsync();
                }
            }

            List<Models.Trailer> trailers = _context.Trailers.Where(v => v.CreatedBy == id).ToList();
            foreach (Models.Trailer? trailer in trailers)
            {
                if (trailer.CreatedBy == id)
                {
                    _context.Trailers.Remove(trailer);
                    _context.SaveChangesAsync();
                }
            }

            List<Models.Boat> boats = _context.Boats.Where(v => v.CreatedBy == id).ToList();
            foreach (Models.Boat? boat in boats)
            {
                if (boat.CreatedBy == id)
                {
                    _context.Boats.Remove(boat);
                    _context.SaveChangesAsync();
                }
            }

            List<Models.Bus> buses = _context.Buses.Where(v => v.CreatedBy == id).ToList();
            foreach (Models.Bus? bus in buses)
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
