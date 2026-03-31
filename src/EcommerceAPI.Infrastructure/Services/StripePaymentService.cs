using EcommerceAPI.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Stripe;

namespace EcommerceAPI.Infrastructure.Services;

public class StripePaymentService : IPaymentService
{
    public StripePaymentService(IConfiguration configuration)
    {
        StripeConfiguration.ApiKey = configuration["Stripe:SecretKey"];
    }

    public async Task<(string ClientSecret, string PaymentIntentId)> CreatePaymentIntentAsync(decimal amount, string currency = "usd")
    {
        var options = new PaymentIntentCreateOptions
        {
            Amount = (long)(amount * 100), // Stripe maneja centavos
            Currency = currency,
            AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
            {
                Enabled = true
            }
        };

        var service = new PaymentIntentService();
        var paymentIntent = await service.CreateAsync(options);

        return (paymentIntent.ClientSecret, paymentIntent.Id);
    }
}
