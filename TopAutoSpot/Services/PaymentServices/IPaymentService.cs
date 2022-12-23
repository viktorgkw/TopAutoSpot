using TopAutoSpot.Models;

namespace TopAutoSpot.Services.PaymentServices
{
    public interface IPaymentService
    {
        Task<string> MakePayment(StripePayment paymentInfo);
    }
}
