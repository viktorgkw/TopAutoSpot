namespace TopAutoSpot.Models
{
    public class Auction
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string Status { get; set; }
        public string VehicleId { get; set; }
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
