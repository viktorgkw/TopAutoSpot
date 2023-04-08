namespace TopAutoSpot.Data.Models
{
    /// <summary>
    /// Represents an image associated with a vehicle in the system.
    /// </summary>
    public class VehicleImage
    {
        /// <summary>
        /// The unique identifier of the image.
        /// </summary>
        public string Id { get; set; } = null!;

        /// <summary>
        /// The name of the image file.
        /// </summary>
        public string ImageName { get; set; } = null!;

        /// <summary>
        /// The binary data of the image file.
        /// </summary>
        public byte[] ImageData { get; set; } = null!;

        /// <summary>
        /// The unique identifier of the vehicle associated with this image.
        /// </summary>
        public string VehicleId { get; set; } = null!;
    }
}
