namespace TopAutoSpot.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Represents a boat in the system.
    /// </summary>
    public class Boat
    {
        /// <summary>
        /// The unique identifier of the boat.
        /// </summary>
        [Key]
        public string Id { get; set; } = null!;

        /// <summary>
        /// The user who created the boat.
        /// </summary>
        public string CreatedBy { get; set; } = null!;

        /// <summary>
        /// The date and time when the boat was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// The location where the boat is located.
        /// </summary>
        public string Location { get; set; } = null!;

        /// <summary>
        /// The title or name of the boat.
        /// </summary>
        public string Title { get; set; } = null!;

        /// <summary>
        /// The description of the boat.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// The price of the boat.
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// The make or brand of the boat.
        /// </summary>
        public string Make { get; set; } = null!;

        /// <summary>
        /// The model of the boat.
        /// </summary>
        public string Model { get; set; } = null!;

        /// <summary>
        /// The manufacture date of the boat.
        /// </summary>
        public DateTime ManufactureDate { get; set; }

        /// <summary>
        /// The horse power of the boat engine.
        /// </summary>
        public int? HorsePower { get; set; }

        /// <summary>
        /// The engine type of the boat.
        /// </summary>
        public string EngineType { get; set; } = null!;

        /// <summary>
        /// The number of engines on the boat.
        /// </summary>
        public int EngineCount { get; set; }

        /// <summary>
        /// The width of the boat.
        /// </summary>
        public double Width { get; set; }

        /// <summary>
        /// The length of the boat.
        /// </summary>
        public double Length { get; set; }

        /// <summary>
        /// The material used to make the boat.
        /// </summary>
        public string? Material { get; set; }

        /// <summary>
        /// The status of the boat.
        /// </summary>
        public string Status { get; set; } = null!;
    }
}
