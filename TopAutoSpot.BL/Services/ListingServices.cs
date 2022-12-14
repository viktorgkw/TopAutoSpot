using Microsoft.EntityFrameworkCore;
using TopAutoSpot.BL.Services.Utilities;
using TopAutoSpot.Data;
using TopAutoSpot.Data.Entities;

namespace TopAutoSpot.BL.Services
{
    public class ListingServices : IService<Listing>
    {
        private ApplicationDbContext _context;
        public ListingServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public async void Add(Listing listing)
        {
            await _context.Listings.AddAsync(listing);
            await _context.SaveChangesAsync();
        }

        public void Delete(Listing listingToDelete)
        {
            DeleteById(listingToDelete.Id);
        }

        public async void DeleteById(string listingToDeleteId)
        {
            var foundListing = await _context.Listings.FirstOrDefaultAsync(l => l.Id == listingToDeleteId);

            if (foundListing != null)
            {
                VehicleRemover.RemoveVehicle(foundListing, _context);

                _context.Listings.Remove(foundListing);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Listing>> GetAll()
        {
            return await _context.Listings.ToListAsync();
        }

        public async Task<Listing> GetById(string listingId)
        {
            return await _context.Listings.FirstOrDefaultAsync(l => l.Id == listingId);
        }

        public async void Update(string listingToUpdateId, Listing updatedListing)
        {
            var foundListing = await _context.Listings.FirstOrDefaultAsync(l => l.Id == listingToUpdateId);

            if (foundListing != null)
            {
                foundListing.Title = updatedListing.Title;
                foundListing.Description = updatedListing.Description;
                foundListing.Price = updatedListing.Price;

                await _context.SaveChangesAsync();
            }
        }

        public void Update(Listing listingToUpdate, Listing updatedListing)
        {
            Update(listingToUpdate.Id, updatedListing);
        }
    }
}
