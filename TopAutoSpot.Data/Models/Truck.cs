namespace TopAutoSpot.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Represents a truck item for sale.
    /// </summary>
    public class Truck
    {
        /// <summary>
        /// Gets or sets the ID of the truck.
        /// </summary>
        [Key]
        public string Id { get; set; } = null!;

        /// <summary>
        /// Gets or sets the name of the user who created the truck item.
        /// </summary>
        public string CreatedBy { get; set; } = null!;

        /// <summary>
        /// Gets or sets the date and time when the truck item was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the location where the truck is available for sale.
        /// </summary>
        public string Location { get; set; } = null!;

        /// <summary>
        /// Gets or sets the title of the truck item.
        /// </summary>
        public string Title { get; set; } = null!;

        /// <summary>
        /// Gets or sets the description of the truck item.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the price of the truck item.
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// Gets or sets the make of the truck.
        /// </summary>
        public string Make { get; set; } = null!;

        /// <summary>
        /// Gets or sets the model of the truck.
        /// </summary>
        public string Model { get; set; } = null!;

        /// <summary>
        /// Gets or sets the date when the truck was manufactured.
        /// </summary>
        public DateTime ManufactureDate { get; set; }

        /// <summary>
        /// Gets or sets the horse power of the truck.
        /// </summary>
        public int? HorsePower { get; set; }

        /// <summary>
        /// Gets or sets the mileage of the truck.
        /// </summary>
        public double? Mileage { get; set; }

        /// <summary>
        /// Gets or sets the transmission type of the truck.
        /// </summary>
        public string Transmission { get; set; } = null!;

        /// <summary>
        /// Gets or sets the engine type of the truck.
        /// </summary>
        public string EngineType { get; set; } = null!;

        /// <summary>
        /// Gets or sets the payload of the truck.
        /// </summary>
        public int Payload { get; set; }

        /// <summary>
        /// Gets or sets the number of axles of the truck.
        /// </summary>
        public int AxlesCount { get; set; }

        /// <summary>
        /// Gets or sets the Euro standard of the truck.
        /// </summary>
        public string EuroStandart { get; set; } = null!;

        /// <summary>
        /// Gets or sets the status of the truck item.
        /// </summary>
        public string Status { get; set; } = null!;
    }
}
