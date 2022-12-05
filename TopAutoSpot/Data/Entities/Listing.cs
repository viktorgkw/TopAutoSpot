using TopAutoSpot.Data.Entities.Abstractions;

namespace TopAutoSpot.Data.Entities
{
    public class Listing
    {
        public Guid Id { get; set; }
        public IVehicle Vehicle { get; set; }
        public string Category { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public double Price { get; set; }
    }
}
