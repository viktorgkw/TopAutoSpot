namespace TopAutoSpot.Data.Models
{
    public class Auction
    {
        public string Id { get; set; } = null!;

        public string Title { get; set; } = null!;

        public string? Description { get; set; }

        public string Status { get; set; } = null!;

        public string VehicleId { get; set; } = null!;

        public int StartingPrice { get; set; }

        public int Duration { get; set; }

        public DateTime StartDay { get; set; }

        public DateTime StartHour { get; set; }

        public string? AuctioneerId { get; set; }

        public List<User>? Bidders { get; set; } = new List<User>();

        public int? CurrentBidPrice { get; set; }

        public string? LastBidderId { get; set; }
    }
}
