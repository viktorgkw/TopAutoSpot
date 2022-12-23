namespace TopAutoSpot.Models
{
    public class StripePayment
    {
        public string Id { get; set; }
        public string CustomerName { get; set; }
        public string Email { get; set; }
        public string CardNumber { get; set; }
        public string ExpYear { get; set; }
        public string ExpMonth { get; set; }
        public string CVC { get; set; }
        public long ChargeAmount { get; set; }
        public string Currency { get; set; }
        public string Description { get; set; }
    }
}
