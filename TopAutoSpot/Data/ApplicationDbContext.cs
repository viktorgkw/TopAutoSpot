using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TopAutoSpot.Data.Entities;
using TopAutoSpot.Data.Entities.Utilities;

namespace TopAutoSpot.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            InitializeAdministrator();
        }

        // Main Tables
        public DbSet<User> Users { get; set; }
        public DbSet<Listing> Listings { get; set; }

        // Not sure if needed tables
        public DbSet<Car> Cars { get; set; }
        public DbSet<Truck> Trucks { get; set; }
        public DbSet<Boat> Boats { get; set; }
        public DbSet<Bus> Buses { get; set; }
        public DbSet<Motorcycle> Motorcycles { get; set; }
        public DbSet<Trailer> Trailers { get; set; }

        private void InitializeAdministrator()
        {
            this.Users.Add(new User()
            {
                Id = new Guid().ToString(),
                PasswordHash = "admin",
                UserName = "admin",
                Email = "admin@gmail.com",
                PhoneNumber = "1234567890",
                FirstName = "admin",
                LastName = "admin",
                Role = RoleTypes.Administrator.ToString(),
                Listings = new List<Listing>(),
            });
        }
    }
}