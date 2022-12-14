using System.ComponentModel.DataAnnotations;

namespace TopAutoSpot.Data.Entities
{
    public class Truck
    {
        [Key]
        public string Id { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public DateTime ManufactoreDate { get; set; }
        public int? HorsePower { get; set; }
        public double? Mileage { get; set; }
        public string Transmission { get; set; }
        public string EngineType { get; set; }
        public int Payload { get; set; }
        public int AxlesCount { get; set; }
        public string EuroStandart { get; set; }
        public string CreatedBy { get; set; }
    }
}
