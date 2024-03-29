﻿namespace TopAutoSpot.Services.PaymentServices
{
    using Stripe;
    using TopAutoSpot.Data.Models;

    /// <summary>
    /// This class is a service class for payments.
    /// </summary>
    public class PaymentService : IPaymentService
    {
        /// <summary>
        /// This class realizes a payment with given payment info.
        /// </summary>
        /// <param name="paymentInfo">Payment Info</param>
        /// <returns>Status of the payment.</returns>
        public string MakePayment(StripePayment paymentInfo)
        {
            TokenCreateOptions tokenOptions = new()
            {
                Card = new TokenCardOptions
                {
                    Number = paymentInfo.CardNumber,
                    ExpYear = paymentInfo.ExpYear,
                    ExpMonth = paymentInfo.ExpMonth,
                    Cvc = paymentInfo.CVC
                }
            };

            TokenService tokenService = new();
            Token token = tokenService.Create(tokenOptions);

            ChargeCreateOptions chargeOptions = new()
            {
                Amount = paymentInfo.ChargeAmount,
                Currency = paymentInfo.Currency,
                Description = paymentInfo.Description,
                Source = token.Id
            };

            ChargeService chargeService = new();
            Charge charge = chargeService.Create(chargeOptions);

            return charge.Status;
        }
    }
}
