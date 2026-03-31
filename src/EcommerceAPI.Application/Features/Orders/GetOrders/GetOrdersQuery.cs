using MediatR;

namespace EcommerceAPI.Application.Features.Orders.GetOrders;

public record GetOrdersQuery(string UserId) : IRequest<List<OrderSummaryDto>>;

public record OrderSummaryDto(
    Guid Id,
    string Status,
    decimal Total,
    int ItemCount,
    DateTime CreatedAt
);
