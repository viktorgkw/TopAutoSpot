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

        public void RemoveAllUserAuctions(string id)
        {
            var auctions = _context.Auctions.Where(a => a.AuctioneerId == id);

            foreach (var auction in auctions)
            {
                _context.Auctions.Remove(auction);
                _context.SaveChangesAsync();
            }
        }

        public void RemoveAllUserVehicles(string id)
        {
            var cars = _context.Cars.Where(v => v.CreatedBy == id).ToList();
            foreach (var car in cars)
            {
                if (car.CreatedBy == id)
                {
                    _context.Cars.Remove(car);
                    _context.SaveChangesAsync();
                }
            }

            var motorcycles = _context.Motorcycles.Where(v => v.CreatedBy == id).ToList();
            foreach (var moto in motorcycles)
            {
                if (moto.CreatedBy == id)
                {
                    _context.Motorcycles.Remove(moto);
                    _context.SaveChangesAsync();
                }
            }

            var trucks = _context.Trucks.Where(v => v.CreatedBy == id).ToList();
            foreach (var truck in trucks)
            {
                if (truck.CreatedBy == id)
                {
                    _context.Trucks.Remove(truck);
                    _context.SaveChangesAsync();
                }
            }

            var trailers = _context.Trailers.Where(v => v.CreatedBy == id).ToList();
            foreach (var trailer in trailers)
            {
                if (trailer.CreatedBy == id)
                {
                    _context.Trailers.Remove(trailer);
                    _context.SaveChangesAsync();
                }
            }

            var boats = _context.Boats.Where(v => v.CreatedBy == id).ToList();
            foreach (var boat in boats)
            {
                if (boat.CreatedBy == id)
                {
                    _context.Boats.Remove(boat);
                    _context.SaveChangesAsync();
                }
            }

            var buses = _context.Buses.Where(v => v.CreatedBy == id).ToList();
            foreach (var bus in buses)
            {
                if (bus.CreatedBy == id)
                {
                    _context.Buses.Remove(bus);
                    _context.SaveChangesAsync();
                }
            }
        }
    }
}
