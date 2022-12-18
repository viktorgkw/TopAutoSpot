using System.ComponentModel.DataAnnotations;

namespace TopAutoSpot.Data.Entities
{
    public class Truck
    {
        [Key]
        public string Id { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Location { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public double Price { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public DateTime ManufactureDate { get; set; }
        public int? HorsePower { get; set; }
        public double? Mileage { get; set; }
        public string Transmission { get; set; }
        public string EngineType { get; set; }
        public int Payload { get; set; }
        public int AxlesCount { get; set; }
        public string EuroStandart { get; set; }
        public string Status { get; set; }
    }
}
