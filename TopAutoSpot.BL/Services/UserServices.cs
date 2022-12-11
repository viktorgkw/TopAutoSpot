using Microsoft.EntityFrameworkCore;
using TopAutoSpot.BL.Services.Utilities;
using TopAutoSpot.Data;
using TopAutoSpot.Data.Entities;

namespace TopAutoSpot.BL.Services
{
    public class UserServices : IService<User>
    {
        public async void Add(User user)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                await db.Users.AddAsync(user);
                await db.SaveChangesAsync();
            }
        }

        public void Delete(User userToDelete)
        {
            DeleteById(userToDelete.Id);
        }

        public async void DeleteById(string userToDeleteId)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var foundUser = await db.Users.FirstOrDefaultAsync(u => u.Id == userToDeleteId);

                if (foundUser != null)
                {
                    var userListings = foundUser.Listings.ToList();

                    foreach (var listing in userListings)
                    {
                        VehicleRemover.RemoveVehicle(listing, db);
                    }

                    foundUser.Listings.RemoveAll(l => l.Id != null);
                    db.Remove(foundUser);
                    await db.SaveChangesAsync();
                }
            }
        }

        public async Task<List<User>> GetAll()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return await db.Users.ToListAsync();
            }
        }

        public async Task<User> GetById(string userId)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return await db.Users.FirstOrDefaultAsync(u => u.Id == userId);
            }
        }

        public async void Update(string userToUpdateId, User updatedUser)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var foundUser = await db.Users.FirstOrDefaultAsync(u => u.Id == userToUpdateId);

                if (foundUser != null)
                {
                    foundUser.Email = updatedUser.Email;
                    foundUser.PhoneNumber = updatedUser.PhoneNumber;
                    foundUser.UserName = updatedUser.UserName;
                    foundUser.FirstName = updatedUser.FirstName;
                    foundUser.LastName = updatedUser.LastName;
                    foundUser.Role = updatedUser.Role;

                    await db.SaveChangesAsync();
                }
            }
        }

        public void Update(User userToUpdate, User updatedUser)
        {
            Update(userToUpdate.Id, updatedUser);
        }
    }
}
