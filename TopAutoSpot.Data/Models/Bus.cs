namespace TopAutoSpot.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Represents a bus for sale.
    /// </summary>
    public class Bus
    {
        /// <summary>
        /// The unique identifier of the bus.
        /// </summary>
        [Key]
        public string Id { get; set; } = null!;

        /// <summary>
        /// The name of the user who created the bus listing.
        /// </summary>
        public string CreatedBy { get; set; } = null!;

        /// <summary>
        /// The date and time when the bus listing was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// The location of the bus.
        /// </summary>
        public string Location { get; set; } = null!;

        /// <summary>
        /// The title of the bus listing.
        /// </summary>
        public string Title { get; set; } = null!;

        /// <summary>
        /// The description of the bus listing.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// The price of the bus.
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// The make of the bus.
        /// </summary>
        public string Make { get; set; } = null!;

        /// <summary>
        /// The model of the bus.
        /// </summary>
        public string Model { get; set; } = null!;

        /// <summary>
        /// The manufacture date of the bus.
        /// </summary>
        public DateTime ManufactureDate { get; set; }

        /// <summary>
        /// The horse power of the bus.
        /// </summary>
        public int? HorsePower { get; set; }

        /// <summary>
        /// The mileage of the bus.
        /// </summary>
        public double? Mileage { get; set; }

        /// <summary>
        /// The transmission type of the bus.
        /// </summary>
        public string Transmission { get; set; } = null!;

        /// <summary>
        /// The engine type of the bus.
        /// </summary>
        public string EngineType { get; set; } = null!;

        /// <summary>
        /// The Euro standard of the bus.
        /// </summary>
        public string EuroStandart { get; set; } = null!;

        /// <summary>
        /// The number of axles of the bus.
        /// </summary>
        public int AxlesCount { get; set; }

        /// <summary>
        /// The number of seats in the bus.
        /// </summary>
        public int SeatsCount { get; set; }

        /// <summary>
        /// The load capacity of the bus.
        /// </summary>
        public double LoadCapacity { get; set; }

        /// <summary>
        /// The status of the bus listing.
        /// </summary>
        public string Status { get; set; } = null!;
    }
}
