using TopAutoSpot.Data.Entities.Abstractions;

namespace TopAutoSpot.Data.Entities
{
    public class Boat : IVehicle
    {
        public Guid Id { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public DateTime ManufactoreDate { get; set; }
        public int? HorsePower { get; set; }
        public string EngineType { get; set; }
        public int EngineCount { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public string? Material { get; set; }
    }
}
