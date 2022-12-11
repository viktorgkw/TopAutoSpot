using Microsoft.EntityFrameworkCore;
using TopAutoSpot.BL.Services.Utilities;
using TopAutoSpot.Data;
using TopAutoSpot.Data.Entities;

namespace TopAutoSpot.BL.Services
{
    public class ListingServices : IService<Listing>
    {
        public async void Add(Listing listing)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                await db.Listings.AddAsync(listing);
                await db.SaveChangesAsync();
            }
        }

        public void Delete(Listing listingToDelete)
        {
            DeleteById(listingToDelete.Id);
        }

        public async void DeleteById(string listingToDeleteId)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var foundListing = await db.Listings.FirstOrDefaultAsync(l => l.Id == listingToDeleteId);

                if (foundListing != null)
                {
                    VehicleRemover.RemoveVehicle(foundListing, db);

                    db.Listings.Remove(foundListing);
                    await db.SaveChangesAsync();
                }
            }
        }

        public async Task<List<Listing>> GetAll()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return await db.Listings.ToListAsync();
            }
        }

        public async Task<Listing> GetById(string listingId)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return await db.Listings.FirstOrDefaultAsync(l => l.Id == listingId);
            }
        }

        public async void Update(string listingToUpdateId, Listing updatedListing)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var foundListing = await db.Listings.FirstOrDefaultAsync(l => l.Id == listingToUpdateId);

                if (foundListing != null)
                {
                    foundListing.Title = updatedListing.Title;
                    foundListing.Description = updatedListing.Description;
                    foundListing.Price = updatedListing.Price;

                    await db.SaveChangesAsync();
                }
            }
        }

        public void Update(Listing listingToUpdate, Listing updatedListing)
        {
            Update(listingToUpdate.Id, updatedListing);
        }
    }
}
