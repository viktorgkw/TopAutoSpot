using System.ComponentModel.DataAnnotations;

namespace TopAutoSpot.Data.Models
{
    public class Boat
    {
        [Key]
        public string Id { get; set; } = null!;

        public string CreatedBy { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public string Location { get; set; } = null!;

        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public double Price { get; set; }

        public string Make { get; set; } = null!;

        public string Model { get; set; } = null!;

        public DateTime ManufactureDate { get; set; }

        public int? HorsePower { get; set; }

        public string EngineType { get; set; } = null!;

        public int EngineCount { get; set; }

        public double Width { get; set; }

        public double Length { get; set; }

        public string? Material { get; set; }

        public string Status { get; set; } = null!;
    }
}
