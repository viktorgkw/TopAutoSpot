namespace TopAutoSpot.Data
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    using TopAutoSpot.Data.Models;

    /// <summary>
    /// Represents the database context for the application.
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        /// <summary>
        /// Represents the DbSet of Users in the database.
        /// </summary>
        public override DbSet<User> Users { get; set; } = null!;

        /// <summary>
        /// Represents the DbSet of Auctions in the database.
        /// </summary>
        public DbSet<Auction> Auctions { get; set; } = null!;

        /// <summary>
        /// Represents the DbSet of InterestedListings in the database.
        /// </summary>
        public DbSet<InterestedListing> InterestedInListings { get; set; } = null!;

        /// <summary>
        /// Represents the DbSet of Notifications in the database.
        /// </summary>
        public DbSet<Notification> Notifications { get; set; } = null!;

        /// <summary>
        /// Represents the DbSet of VehicleImages in the database.
        /// </summary>
        public DbSet<VehicleImage> VehicleImages { get; set; } = null!;

        /// <summary>
        /// Represents the DbSet of Cars in the database.
        /// </summary>
        public DbSet<Car> Cars { get; set; } = null!;

        /// <summary>
        /// Represents the DbSet of Trucks in the database.
        /// </summary>
        public DbSet<Truck> Trucks { get; set; } = null!;

        /// <summary>
        /// Represents the DbSet of Boats in the database.
        /// </summary>
        public DbSet<Boat> Boats { get; set; } = null!;

        /// <summary>
        /// Represents the DbSet of Buses in the database.
        /// </summary>
        public DbSet<Bus> Buses { get; set; } = null!;

        /// <summary>
        /// Represents the DbSet of Motorcycles in the database.
        /// </summary>
        public DbSet<Motorcycle> Motorcycles { get; set; } = null!;

        /// <summary>
        /// Represents the DbSet of Trailers in the database.
        /// </summary>
        public DbSet<Trailer> Trailers { get; set; } = null!;
    }
}