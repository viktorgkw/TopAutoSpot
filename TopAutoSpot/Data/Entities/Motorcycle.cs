namespace TopAutoSpot.Data.Entities
{
    public class Motorcycle
    {
        public Guid Id { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public DateTime ManufactoreDate { get; set; }
        public int? HorsePower { get; set; }
        public double? Mileage { get; set; }
        public string Transmission { get; set; }
        public string EngineType { get; set; }
        public double CubicCapacity { get; set; }
        public int EngineStrokes { get; set; }
        public string CoolingType { get; set; }
    }
}
