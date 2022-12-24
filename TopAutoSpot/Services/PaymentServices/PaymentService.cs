using Stripe;
using TopAutoSpot.Models;

namespace TopAutoSpot.Services.PaymentServices
{
    public class PaymentService : IPaymentService
    {
        public string MakePayment(StripePayment paymentInfo)
        {
            TokenCreateOptions tokenOptions = new TokenCreateOptions
            {
                Card = new TokenCardOptions
                {
                    Number = paymentInfo.CardNumber,
                    ExpYear = paymentInfo.ExpYear,
                    ExpMonth = paymentInfo.ExpMonth,
                    Cvc = paymentInfo.CVC
                }
            };

            TokenService tokenService = new TokenService();
            Token token = tokenService.Create(tokenOptions);

            ChargeCreateOptions chargeOptions = new ChargeCreateOptions
            {
                Amount = paymentInfo.ChargeAmount,
                Currency = paymentInfo.Currency,
                Description = paymentInfo.Description,
                Source = token.Id
            };

            ChargeService chargeService = new ChargeService();
            Charge charge = chargeService.Create(chargeOptions);

            return charge.Status;
        }
    }
}
