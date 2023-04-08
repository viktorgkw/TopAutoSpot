namespace TopAutoSpot.Data.Models
{
    /// <summary>
    /// Class representing an auction for a vehicle.
    /// </summary>
    public class Auction
    {
        /// <summary>
        /// The unique identifier of the auction.
        /// </summary>
        public string Id { get; set; } = null!;

        /// <summary>
        /// The title of the auction.
        /// </summary>
        public string Title { get; set; } = null!;

        /// <summary>
        /// The description of the auction.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// The status of the auction, such as "active" or "closed".
        /// </summary>
        public string Status { get; set; } = null!;

        /// <summary>
        /// The unique identifier of the vehicle being auctioned.
        /// </summary>
        public string VehicleId { get; set; } = null!;

        /// <summary>
        /// The starting price of the auction.
        /// </summary>
        public int StartingPrice { get; set; }

        /// <summary>
        /// The duration of the auction in minutes.
        /// </summary>
        public int Duration { get; set; }

        /// <summary>
        /// The starting date of the auction.
        /// </summary>
        public DateTime StartDay { get; set; }

        /// <summary>
        /// The starting time of the auction.
        /// </summary>
        public DateTime StartHour { get; set; }

        /// <summary>
        /// The unique identifier of the user who is running the auction.
        /// </summary>
        public string? AuctioneerId { get; set; }

        /// <summary>
        /// The list of users who have placed bids on the auction.
        /// </summary>
        public List<User>? Bidders { get; set; } = new List<User>();

        /// <summary>
        /// The current bid price of the auction.
        /// </summary>
        public int? CurrentBidPrice { get; set; }

        /// <summary>
        /// The unique identifier of the user who placed the last bid.
        /// </summary>
        public string? LastBidderId { get; set; }
    }
}
