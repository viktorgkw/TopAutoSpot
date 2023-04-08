namespace TopAutoSpot.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Represents a motorcycle.
    /// </summary>
    public class Motorcycle
    {
        /// <summary>
        /// Gets or sets the unique identifier of the motorcycle.
        /// </summary>
        [Key]
        public string Id { get; set; } = null!;

        /// <summary>
        /// Gets or sets the name of the user who created the motorcycle listing.
        /// </summary>
        public string CreatedBy { get; set; } = null!;

        /// <summary>
        /// Gets or sets the date and time when the motorcycle listing was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the location where the motorcycle is located.
        /// </summary>
        public string Location { get; set; } = null!;

        /// <summary>
        /// Gets or sets the title of the motorcycle listing.
        /// </summary>
        public string Title { get; set; } = null!;

        /// <summary>
        /// Gets or sets the description of the motorcycle listing.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the price of the motorcycle.
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// Gets or sets the make of the motorcycle.
        /// </summary>
        public string Make { get; set; } = null!;

        /// <summary>
        /// Gets or sets the model of the motorcycle.
        /// </summary>
        public string Model { get; set; } = null!;

        /// <summary>
        /// Gets or sets the date when the motorcycle was manufactured.
        /// </summary>
        public DateTime ManufactureDate { get; set; }

        /// <summary>
        /// Gets or sets the horsepower of the motorcycle.
        /// </summary>
        public int? HorsePower { get; set; }

        /// <summary>
        /// Gets or sets the mileage of the motorcycle.
        /// </summary>
        public double? Mileage { get; set; }

        /// <summary>
        /// Gets or sets the transmission type of the motorcycle.
        /// </summary>
        public string Transmission { get; set; } = null!;

        /// <summary>
        /// Gets or sets the engine type of the motorcycle.
        /// </summary>
        public string EngineType { get; set; } = null!;

        /// <summary>
        /// Gets or sets the cubic capacity of the motorcycle's engine.
        /// </summary>
        public double CubicCapacity { get; set; }

        /// <summary>
        /// Gets or sets the number of engine strokes of the motorcycle.
        /// </summary>
        public int EngineStrokes { get; set; }

        /// <summary>
        /// Gets or sets the cooling type of the motorcycle.
        /// </summary>
        public string CoolingType { get; set; } = null!;

        /// <summary>
        /// Gets or sets the status of the motorcycle listing.
        /// </summary>
        public string Status { get; set; } = null!;
    }
}
