namespace TopAutoSpot.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Represents a Car entity in the database.
    /// </summary>
    public class Car
    {
        /// <summary>
        /// The unique identifier for the car.
        /// </summary>
        [Key]
        public string Id { get; set; } = null!;

        /// <summary>
        /// The id of the user who created the car listing.
        /// </summary>
        public string CreatedBy { get; set; } = null!;

        /// <summary>
        /// The date and time when the car listing was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// The location of the car.
        /// </summary>
        public string Location { get; set; } = null!;

        /// <summary>
        /// The title of the car listing.
        /// </summary>
        public string Title { get; set; } = null!;

        /// <summary>
        /// The description of the car listing.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// The price of the car.
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// The make of the car.
        /// </summary>
        public string Make { get; set; } = null!;

        /// <summary>
        /// The model of the car.
        /// </summary>
        public string Model { get; set; } = null!;

        /// <summary>
        /// The manufacture date of the car.
        /// </summary>
        public DateTime ManufactureDate { get; set; }

        /// <summary>
        /// The horse power of the car.
        /// </summary>
        public int? HorsePower { get; set; }

        /// <summary>
        /// The mileage of the car.
        /// </summary>
        public double? Mileage { get; set; }

        /// <summary>
        /// The transmission type of the car.
        /// </summary>
        public string Transmission { get; set; } = null!;

        /// <summary>
        /// The engine type of the car.
        /// </summary>
        public string EngineType { get; set; } = null!;

        /// <summary>
        /// The Euro standart of the car.
        /// </summary>
        public string EuroStandart { get; set; } = null!;

        /// <summary>
        /// The status of the car listing.
        /// </summary>
        public string Status { get; set; } = null!;
    }
}
