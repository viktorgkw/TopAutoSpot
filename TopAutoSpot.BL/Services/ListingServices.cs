using TopAutoSpot.BL.Services.Utilities;
using TopAutoSpot.Data;
using TopAutoSpot.Data.Entities;

namespace TopAutoSpot.BL.Services
{
    public class ListingServices : IService<Listing>
    {
        public void Add(Listing listing)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                db.Listings.Add(listing);
                db.SaveChanges();
            }
        }

        public void Delete(Listing listingToDelete)
        {
            DeleteById(listingToDelete.Id);
        }

        public void DeleteById(string listingToDeleteId)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var foundListing = db.Listings.FirstOrDefault(l => l.Id == listingToDeleteId);

                if (foundListing != null)
                {
                    VehicleRemover.RemoveVehicle(foundListing, db);

                    db.Listings.Remove(foundListing);
                    db.SaveChanges();
                }
            }
        }

        public List<Listing> GetAll()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return db.Listings.ToList();
            }
        }

        public Listing GetById(string listingId)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return db.Listings.FirstOrDefault(l => l.Id == listingId);
            }
        }

        public void Update(string listingToUpdateId, Listing updatedListing)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var foundListing = db.Listings.FirstOrDefault(l => l.Id == listingToUpdateId);

                if (foundListing != null)
                {
                    foundListing.Title = updatedListing.Title;
                    foundListing.Description = updatedListing.Description;
                    foundListing.Price = updatedListing.Price;

                    db.SaveChanges();
                }
            }
        }

        public void Update(Listing listingToUpdate, Listing updatedListing)
        {
            Update(listingToUpdate.Id, updatedListing);
        }
    }
}
