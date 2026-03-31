using MediatR;

namespace EcommerceAPI.Application.Features.Payments.CreatePaymentIntent;

public record CreatePaymentIntentCommand(Guid OrderId, string UserId) : IRequest<CreatePaymentIntentResponse>;

public record CreatePaymentIntentResponse(string ClientSecret, string PaymentIntentId);
