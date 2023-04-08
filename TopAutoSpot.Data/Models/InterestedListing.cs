﻿namespace TopAutoSpot.Data.Models
{
    /// <summary>
    /// Represents a user's interest in a particular vehicle listing.
    /// </summary>
    public class InterestedListing
    {
        /// <summary>
        /// The ID of the interested listing.
        /// </summary>
        public string Id { get; set; } = null!;

        /// <summary>
        /// The ID of the user who is interested in the listing.
        /// </summary>
        public string UserId { get; set; } = null!;

        /// <summary>
        /// The ID of the vehicle listing that the user is interested in.
        /// </summary>
        public string VehicleId { get; set; } = null!;

        /// <summary>
        /// The category of the vehicle that the user is interested in.
        /// </summary>
        public string VehicleCategory { get; set; } = null!;
    }
}
