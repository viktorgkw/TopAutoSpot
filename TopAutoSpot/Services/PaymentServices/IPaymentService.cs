using TopAutoSpot.Models;

namespace TopAutoSpot.Services.PaymentServices
{
    public interface IPaymentService
    {
        string MakePayment(StripePayment paymentInfo);
    }
}
