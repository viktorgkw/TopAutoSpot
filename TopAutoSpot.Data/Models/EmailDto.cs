namespace TopAutoSpot.Data.Models
{
    /// <summary>
    /// Represents an email data transfer object containing the email's recipient, subject, and body.
    /// </summary>
    public class EmailDto
    {
        /// <summary>
        /// The email recipient.
        /// </summary>
        public string To { get; set; } = null!;

        /// <summary>
        /// The email subject.
        /// </summary>
        public string Subject { get; set; } = null!;

        /// <summary>
        /// The email body.
        /// </summary>
        public string Body { get; set; } = null!;
    }
}
