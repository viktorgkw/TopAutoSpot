using Stripe;
using TopAutoSpot.Models;

namespace TopAutoSpot.Services.PaymentServices
{
    public class PaymentService : IPaymentService
    {
        public async Task<string> MakePayment(StripePayment paymentInfo)
        {
            var tokenOptions = new TokenCreateOptions
            {
                Card = new TokenCardOptions
                {
                    Number = paymentInfo.CardNumber,
                    ExpYear = paymentInfo.ExpYear,
                    ExpMonth = paymentInfo.ExpMonth,
                    Cvc = paymentInfo.CVC
                }
            };

            var tokenService = new TokenService();
            var token = tokenService.Create(tokenOptions);

            var chargeOptions = new ChargeCreateOptions
            {
                Amount = paymentInfo.ChargeAmount,
                Currency = paymentInfo.Currency,
                Description = paymentInfo.Description,
                Source = token.Id
            };

            var chargeService = new ChargeService();
            var charge = chargeService.Create(chargeOptions);

            return charge.Status;
        }
    }
}
