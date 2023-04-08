namespace TopAutoSpot.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Represents a truck item for sale.
    /// </summary>
    public class Truck
    {
        /// <summary>
        /// The ID of the truck.
        /// </summary>
        [Key]
        public string Id { get; set; } = null!;

        /// <summary>
        /// The name of the user who created the truck item.
        /// </summary>
        public string CreatedBy { get; set; } = null!;

        /// <summary>
        /// The date and time when the truck item was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// The location where the truck is available for sale.
        /// </summary>
        public string Location { get; set; } = null!;

        /// <summary>
        /// The title of the truck item.
        /// </summary>
        public string Title { get; set; } = null!;

        /// <summary>
        /// The description of the truck item.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// The price of the truck item.
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// The make of the truck.
        /// </summary>
        public string Make { get; set; } = null!;

        /// <summary>
        /// The model of the truck.
        /// </summary>
        public string Model { get; set; } = null!;

        /// <summary>
        /// The date when the truck was manufactured.
        /// </summary>
        public DateTime ManufactureDate { get; set; }

        /// <summary>
        /// The horse power of the truck.
        /// </summary>
        public int? HorsePower { get; set; }

        /// <summary>
        /// The mileage of the truck.
        /// </summary>
        public double? Mileage { get; set; }

        /// <summary>
        /// The transmission type of the truck.
        /// </summary>
        public string Transmission { get; set; } = null!;

        /// <summary>
        /// The engine type of the truck.
        /// </summary>
        public string EngineType { get; set; } = null!;

        /// <summary>
        /// The payload of the truck.
        /// </summary>
        public int Payload { get; set; }

        /// <summary>
        /// The number of axles of the truck.
        /// </summary>
        public int AxlesCount { get; set; }

        /// <summary>
        /// The Euro standard of the truck.
        /// </summary>
        public string EuroStandart { get; set; } = null!;

        /// <summary>
        /// The status of the truck item.
        /// </summary>
        public string Status { get; set; } = null!;
    }
}
