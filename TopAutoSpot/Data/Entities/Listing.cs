using System.ComponentModel.DataAnnotations;

namespace TopAutoSpot.Data.Entities
{
    public class Listing
    {
        [Key]
        public string Id { get; set; }
        public string VehicleId { get; set; }
        public string Category { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public double Price { get; set; }
    }
}
