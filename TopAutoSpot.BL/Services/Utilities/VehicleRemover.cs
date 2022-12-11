using TopAutoSpot.Data.Entities.Utilities;
using TopAutoSpot.Data.Entities;
using TopAutoSpot.Data;
using Microsoft.EntityFrameworkCore;

namespace TopAutoSpot.BL.Services.Utilities
{
    public static class VehicleRemover
    {
        public static async void RemoveVehicle(Listing listing, ApplicationDbContext db)
        {
            if (listing.Category == Categories.Cars.ToString())
            {
                var foundCar = await db.Cars.FirstAsync(car => car.Id == listing.VehicleId);
                db.Cars.Remove(foundCar);
                await db.SaveChangesAsync();
            }
            else if (listing.Category == Categories.Buses.ToString())
            {
                var foundBus = await db.Buses.FirstAsync(bus => bus.Id == listing.VehicleId);
                db.Buses.Remove(foundBus);
                await db.SaveChangesAsync();
            }
            else if (listing.Category == Categories.Trailers.ToString())
            {
                var foundTrailer = await db.Trailers.FirstAsync(trailer => trailer.Id == listing.VehicleId);
                db.Trailers.Remove(foundTrailer);
                await db.SaveChangesAsync();
            }
            else if (listing.Category == Categories.Trucks.ToString())
            {
                var foundTruck = await db.Trucks.FirstAsync(truck => truck.Id == listing.VehicleId);
                db.Trucks.Remove(foundTruck);
                await db.SaveChangesAsync();
            }
            else if (listing.Category == Categories.Boats.ToString())
            {
                var foundBoat = await db.Boats.FirstAsync(boat => boat.Id == listing.VehicleId);
                db.Boats.Remove(foundBoat);
                await db.SaveChangesAsync();
            }
            else if (listing.Category == Categories.Motorcycles.ToString())
            {
                var foundMotorcycle = await db.Motorcycles.FirstAsync(motorcycle => motorcycle.Id == listing.VehicleId);
                db.Motorcycles.Remove(foundMotorcycle);
                await db.SaveChangesAsync();
            }
        }
    }
}
