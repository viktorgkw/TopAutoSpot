namespace TopAutoSpot.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Represents a motorcycle.
    /// </summary>
    public class Motorcycle
    {
        /// <summary>
        /// The unique identifier of the motorcycle.
        /// </summary>
        [Key]
        public string Id { get; set; } = null!;

        /// <summary>
        /// The name of the user who created the motorcycle listing.
        /// </summary>
        public string CreatedBy { get; set; } = null!;

        /// <summary>
        /// The date and time when the motorcycle listing was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// The location where the motorcycle is located.
        /// </summary>
        public string Location { get; set; } = null!;

        /// <summary>
        /// The title of the motorcycle listing.
        /// </summary>
        public string Title { get; set; } = null!;

        /// <summary>
        /// The description of the motorcycle listing.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// The price of the motorcycle.
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// The make of the motorcycle.
        /// </summary>
        public string Make { get; set; } = null!;

        /// <summary>
        /// The model of the motorcycle.
        /// </summary>
        public string Model { get; set; } = null!;

        /// <summary>
        /// The date when the motorcycle was manufactured.
        /// </summary>
        public DateTime ManufactureDate { get; set; }

        /// <summary>
        /// The horsepower of the motorcycle.
        /// </summary>
        public int? HorsePower { get; set; }

        /// <summary>
        /// The mileage of the motorcycle.
        /// </summary>
        public double? Mileage { get; set; }

        /// <summary>
        /// The transmission type of the motorcycle.
        /// </summary>
        public string Transmission { get; set; } = null!;

        /// <summary>
        /// The engine type of the motorcycle.
        /// </summary>
        public string EngineType { get; set; } = null!;

        /// <summary>
        /// The cubic capacity of the motorcycle's engine.
        /// </summary>
        public double CubicCapacity { get; set; }

        /// <summary>
        /// The number of engine strokes of the motorcycle.
        /// </summary>
        public int EngineStrokes { get; set; }

        /// <summary>
        /// The cooling type of the motorcycle.
        /// </summary>
        public string CoolingType { get; set; } = null!;

        /// <summary>
        /// The status of the motorcycle listing.
        /// </summary>
        public string Status { get; set; } = null!;
    }
}
