using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using TopAutoSpot.Data.Models;

namespace TopAutoSpot.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public override DbSet<User> Users { get; set; } = null!;

        public DbSet<Auction> Auctions { get; set; } = null!;

        public DbSet<InterestedListing> InterestedInListings { get; set; } = null!;

        public DbSet<Notification> Notifications { get; set; } = null!;

        public DbSet<VehicleImage> VehicleImages { get; set; } = null!;

        public DbSet<Car> Cars { get; set; } = null!;

        public DbSet<Truck> Trucks { get; set; } = null!;

        public DbSet<Boat> Boats { get; set; } = null!;

        public DbSet<Bus> Buses { get; set; } = null!;

        public DbSet<Motorcycle> Motorcycles { get; set; } = null!;

        public DbSet<Trailer> Trailers { get; set; } = null!;
    }
}