namespace TopAutoSpot.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Represents a trailer for sale.
    /// </summary>
    public class Trailer
    {
        /// <summary>
        /// The unique identifier for the trailer.
        /// </summary>
        [Key]
        public string Id { get; set; } = null!;

        /// <summary>
        /// The username of the user who created the trailer listing.
        /// </summary>
        public string CreatedBy { get; set; } = null!;

        /// <summary>
        /// The date and time when the trailer listing was created.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// The location of the trailer.
        /// </summary>
        public string Location { get; set; } = null!;

        /// <summary>
        /// The title of the trailer listing.
        /// </summary>
        public string Title { get; set; } = null!;

        /// <summary>
        /// The description of the trailer listing.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// The price of the trailer.
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// The manufacture date of the trailer.
        /// </summary>
        public DateTime ManufactureDate { get; set; }

        /// <summary>
        /// The maximum payload of the trailer.
        /// </summary>
        public double Payload { get; set; }

        /// <summary>
        /// The number of axles of the trailer.
        /// </summary>
        public int AxleCount { get; set; }

        /// <summary>
        /// The status of the trailer listing.
        /// </summary>
        public string Status { get; set; } = null!;
    }
}
