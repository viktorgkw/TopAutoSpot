namespace TopAutoSpot.Data.Models
{
    /// <summary>
    /// Represents a notification sent from one user to another.
    /// </summary>
    public class Notification
    {
        /// <summary>
        /// This property represents the ID of the notification.
        /// </summary>
        public string Id { get; set; } = null!;

        /// <summary>
        /// This property represents the username/email that sent the notification.
        /// </summary>
        public string From { get; set; } = null!;

        /// <summary>
        /// This property represents the username/email that will recieve the notification.
        /// </summary>
        public string To { get; set; } = null!;

        /// <summary>
        /// This property represents the Title of the notification.
        /// </summary>
        public string Title { get; set; } = null!;

        /// <summary>
        /// This property represents the Description of the notification.
        /// </summary>
        public string Description { get; set; } = null!;

        /// <summary>
        /// This property represents the date and time that the notification was created at.
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}