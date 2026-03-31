using MediatR;

namespace EcommerceAPI.Application.Features.Orders.CreateOrder;

public record CreateOrderCommand(string UserId) : IRequest<CreateOrderResponse>;

public record CreateOrderResponse(Guid OrderId, decimal Total);
