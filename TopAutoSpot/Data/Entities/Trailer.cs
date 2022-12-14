using System.ComponentModel.DataAnnotations;

namespace TopAutoSpot.Data.Entities
{
    public class Trailer
    {
        [Key]
        public string Id { get; set; }
        public DateTime ManufactoreDate { get; set; }
        public double Payload { get; set; }
        public int AxleCount { get; set; }
        public string CreatedBy { get; set; }
    }
}
