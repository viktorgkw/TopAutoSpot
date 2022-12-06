using TopAutoSpot.Data.Entities.Utilities;
using TopAutoSpot.Data.Entities;
using TopAutoSpot.Data;

namespace TopAutoSpot.BL.Services.Utilities
{
    public static class VehicleRemover
    {
        public static void RemoveVehicle(Listing listing, ApplicationDbContext db)
        {
            if (listing.Category == Categories.Cars.ToString())
            {
                var foundCar = db.Cars.First(car => car.Id == listing.VehicleId);
                db.Cars.Remove(foundCar);
                db.SaveChanges();
            }
            else if (listing.Category == Categories.Bus.ToString())
            {
                var foundBus = db.Buses.First(bus => bus.Id == listing.VehicleId);
                db.Buses.Remove(foundBus);
                db.SaveChanges();
            }
            else if (listing.Category == Categories.Trailers.ToString())
            {
                var foundTrailer = db.Trailers.First(trailer => trailer.Id == listing.VehicleId);
                db.Trailers.Remove(foundTrailer);
                db.SaveChanges();
            }
            else if (listing.Category == Categories.Trucks.ToString())
            {
                var foundTruck = db.Trucks.First(truck => truck.Id == listing.VehicleId);
                db.Trucks.Remove(foundTruck);
                db.SaveChanges();
            }
            else if (listing.Category == Categories.Boats.ToString())
            {
                var foundBoat = db.Boats.First(boat => boat.Id == listing.VehicleId);
                db.Boats.Remove(foundBoat);
                db.SaveChanges();

            }
            else if (listing.Category == Categories.Motorcycle.ToString())
            {
                var foundMotorcycle = db.Motorcycles.First(motorcycle => motorcycle.Id == listing.VehicleId);
                db.Motorcycles.Remove(foundMotorcycle);
                db.SaveChanges();
            }
        }
    }
}
