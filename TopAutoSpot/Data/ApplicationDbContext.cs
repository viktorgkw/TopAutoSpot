using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TopAutoSpot.Data.Entities;

namespace TopAutoSpot.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            // It turns out I cannot ensure that we are always going to have Administrator Account,
            // because the only password related property we can access is PasswordHash,
            // which means that the password must be hashed, but I can't hash it the Microsoft way,
            // so I created one by hand.

            // Administrator UserName => admin
            // Administrator Password => @Administrator1
        }

        public ApplicationDbContext() { }

        // Main Tables
        public override DbSet<User> Users { get; set; }
        public DbSet<Listing> Listings { get; set; }

        // Not sure if needed tables
        public DbSet<Car> Cars { get; set; }
        public DbSet<Truck> Trucks { get; set; }
        public DbSet<Boat> Boats { get; set; }
        public DbSet<Bus> Buses { get; set; }
        public DbSet<Motorcycle> Motorcycles { get; set; }
        public DbSet<Trailer> Trailers { get; set; }
    }
}