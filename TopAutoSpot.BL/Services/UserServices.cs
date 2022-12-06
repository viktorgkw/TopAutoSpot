﻿using TopAutoSpot.BL.Services.Utilities;
using TopAutoSpot.Data;
using TopAutoSpot.Data.Entities;

namespace TopAutoSpot.BL.Services
{
    public class UserServices : IService<User>
    {
        public void Add(User user)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                db.Users.Add(user);
                db.SaveChanges();
            }
        }

        public void Delete(User userToDelete)
        {
            DeleteById(userToDelete.Id);
        }

        public void DeleteById(string userToDeleteId)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var foundUser = db.Users.FirstOrDefault(u => u.Id == userToDeleteId);

                if (foundUser != null)
                {
                    var userListings = foundUser.Listings.ToList();

                    foreach (var listing in userListings)
                    {
                        VehicleRemover.RemoveVehicle(listing, db);
                    }

                    foundUser.Listings.RemoveAll(l => l.Id != null);
                    db.Remove(foundUser);
                    db.SaveChanges();
                }
            }
        }

        public List<User> GetAll()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return db.Users.ToList();
            }
        }

        public User GetById(string userId)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                return db.Users.FirstOrDefault(u => u.Id == userId);
            }
        }

        public void Update(string userToUpdateId, User updatedUser)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var foundUser = db.Users.FirstOrDefault(u => u.Id == userToUpdateId);

                if (foundUser != null)
                {
                    foundUser.Email = updatedUser.Email;
                    foundUser.PhoneNumber = updatedUser.PhoneNumber;
                    foundUser.UserName = updatedUser.UserName;
                    foundUser.FirstName = updatedUser.FirstName;
                    foundUser.LastName = updatedUser.LastName;
                    foundUser.Role = updatedUser.Role;
                }
            }
        }

        public void Update(User userToUpdate, User updatedUser)
        {
            Update(userToUpdate.Id, updatedUser);
        }
    }
}
