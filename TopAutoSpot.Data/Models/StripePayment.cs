namespace TopAutoSpot.Data.Models
{
    /// <summary>
    /// Class representing a Stripe payment.
    /// </summary>
    public class StripePayment
    {
        /// <summary>
        /// Gets or sets the ID of the payment.
        /// </summary>
        public string Id { get; set; } = null!;

        /// <summary>
        /// Gets or sets the name of the customer making the payment.
        /// </summary>
        public string CustomerName { get; set; } = null!;

        /// <summary>
        /// Gets or sets the email of the customer making the payment.
        /// </summary>
        public string Email { get; set; } = null!;

        /// <summary>
        /// Gets or sets the card number used for the payment.
        /// </summary>
        public string CardNumber { get; set; } = null!;

        /// <summary>
        /// Gets or sets the expiration year of the card used for the payment.
        /// </summary>
        public string ExpYear { get; set; } = null!;

        /// <summary>
        /// Gets or sets the expiration month of the card used for the payment.
        /// </summary>
        public string ExpMonth { get; set; } = null!;

        /// <summary>
        /// Gets or sets the CVC code of the card used for the payment.
        /// </summary>
        public string CVC { get; set; } = null!;

        /// <summary>
        /// Gets or sets the amount charged for the payment.
        /// </summary>
        public long ChargeAmount { get; set; }

        /// <summary>
        /// Gets or sets the currency used for the payment.
        /// </summary>
        public string Currency { get; set; } = null!;

        /// <summary>
        /// Gets or sets the description of the payment.
        /// </summary>
        public string Description { get; set; } = null!;
    }
}
