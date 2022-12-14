using Microsoft.EntityFrameworkCore;
using TopAutoSpot.BL.Services.Utilities;
using TopAutoSpot.Data;
using TopAutoSpot.Data.Entities;

namespace TopAutoSpot.BL.Services
{
    public class UserServices : IService<User>
    {
        private ApplicationDbContext _context;
        public UserServices(ApplicationDbContext context)
        {
            _context = context;
        }

        public async void Add(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public void Delete(User userToDelete)
        {
            DeleteById(userToDelete.Id);
        }

        public async void DeleteById(string userToDeleteId)
        {
            var foundUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == userToDeleteId);

            if (foundUser != null)
            {
                var userListings = foundUser.Listings.ToList();

                foreach (var listing in userListings)
                {
                    VehicleRemover.RemoveVehicle(listing, _context);
                }

                foundUser.Listings.RemoveAll(l => l.Id != null);
                _context.Remove(foundUser);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<User>> GetAll()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetById(string userId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async void Update(string userToUpdateId, User updatedUser)
        {
            var foundUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == userToUpdateId);

            if (foundUser != null)
            {
                foundUser.Email = updatedUser.Email;
                foundUser.PhoneNumber = updatedUser.PhoneNumber;
                foundUser.UserName = updatedUser.UserName;
                foundUser.FirstName = updatedUser.FirstName;
                foundUser.LastName = updatedUser.LastName;
                foundUser.Role = updatedUser.Role;

                await _context.SaveChangesAsync();
            }
        }

        public void Update(User userToUpdate, User updatedUser)
        {
            Update(userToUpdate.Id, updatedUser);
        }
    }
}
