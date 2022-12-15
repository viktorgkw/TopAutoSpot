using System.ComponentModel.DataAnnotations;

namespace TopAutoSpot.Data.Entities
{
    public class Boat
    {
        [Key]
        public string Id { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public double Price { get; set; }
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
