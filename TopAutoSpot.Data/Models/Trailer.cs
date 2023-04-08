namespace TopAutoSpot.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Represents a trailer for sale.
    /// </summary>
    public class Trailer
    {
        /// <summary>
        /// Gets or sets the unique identifier for the trailer.
        /// </summary>
        [Key]
        public string Id { get; set; } = null!;

        /// <summary>
        /// Gets or sets the username of the user who created the trailer listing.
        /// </summary>
        public string CreatedBy { get; set; } = null!;

        /// <summary>
        /// Gets or sets the date and time when the trailer listing was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the location of the trailer.
        /// </summary>
        public string Location { get; set; } = null!;

        /// <summary>
        /// Gets or sets the title of the trailer listing.
        /// </summary>
        public string Title { get; set; } = null!;

        /// <summary>
        /// Gets or sets the description of the trailer listing.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the price of the trailer.
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// Gets or sets the manufacture date of the trailer.
        /// </summary>
        public DateTime ManufactureDate { get; set; }

        /// <summary>
        /// Gets or sets the maximum payload of the trailer.
        /// </summary>
        public double Payload { get; set; }

        /// <summary>
        /// Gets or sets the number of axles of the trailer.
        /// </summary>
        public int AxleCount { get; set; }

        /// <summary>
        /// Gets or sets the status of the trailer listing.
        /// </summary>
        public string Status { get; set; } = null!;
    }
}
