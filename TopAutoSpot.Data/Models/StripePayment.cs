namespace TopAutoSpot.Data.Models
{
    /// <summary>
    /// Class representing a Stripe payment.
    /// </summary>
    public class StripePayment
    {
        /// <summary>
        /// The ID of the payment.
        /// </summary>
        public string Id { get; set; } = null!;

        /// <summary>
        /// The name of the customer making the payment.
        /// </summary>
        public string CustomerName { get; set; } = null!;

        /// <summary>
        /// The email of the customer making the payment.
        /// </summary>
        public string Email { get; set; } = null!;

        /// <summary>
        /// The card number used for the payment.
        /// </summary>
        public string CardNumber { get; set; } = null!;

        /// <summary>
        /// The expiration year of the card used for the payment.
        /// </summary>
        public string ExpYear { get; set; } = null!;

        /// <summary>
        /// The expiration month of the card used for the payment.
        /// </summary>
        public string ExpMonth { get; set; } = null!;

        /// <summary>
        /// The CVC code of the card used for the payment.
        /// </summary>
        public string CVC { get; set; } = null!;

        /// <summary>
        /// The amount charged for the payment.
        /// </summary>
        public long ChargeAmount { get; set; }

        /// <summary>
        /// The currency used for the payment.
        /// </summary>
        public string Currency { get; set; } = null!;

        /// <summary>
        /// The description of the payment.
        /// </summary>
        public string Description { get; set; } = null!;
    }
}
