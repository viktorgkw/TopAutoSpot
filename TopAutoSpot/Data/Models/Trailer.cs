using System.ComponentModel.DataAnnotations;

namespace TopAutoSpot.Data.Models
{
    public class Trailer
    {
        [Key]
        public string Id { get; set; } = null!;
        public string CreatedBy { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public string Location { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public double Price { get; set; }
        public DateTime ManufactureDate { get; set; }
        public double Payload { get; set; }
        public int AxleCount { get; set; }
        public string Status { get; set; } = null!;
    }
}
