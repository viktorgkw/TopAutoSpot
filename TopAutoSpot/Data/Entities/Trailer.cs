using TopAutoSpot.Data.Entities.Abstractions;

namespace TopAutoSpot.Data.Entities
{
    public class Trailer : IVehicle
    {
        public Guid Id { get; set; }
        public DateTime ManufactoreDate { get; set; }
        public double Payload { get; set; }
        public int AxleCount { get; set; }
    }
}
