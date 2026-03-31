namespace EcommerceAPI.Application.Common.Interfaces;

public interface IPaymentService
{
    Task<(string ClientSecret, string PaymentIntentId)> CreatePaymentIntentAsync(decimal amount, string currency = "usd");
}
