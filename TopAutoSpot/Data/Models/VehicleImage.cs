namespace TopAutoSpot.Data.Models
{
    public class VehicleImage
    {
        public string Id { get; set; } = null!;

        public string ImageName { get; set; } = null!;

        public byte[] ImageData { get; set; } = null!;

        public string VehicleId { get; set; } = null!;
    }
}
