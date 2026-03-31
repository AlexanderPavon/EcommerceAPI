using MediatR;

namespace EcommerceAPI.Application.Features.Orders.GetOrderById;

public record GetOrderByIdQuery(Guid OrderId, string UserId) : IRequest<OrderDetailDto?>;

public record OrderDetailDto(
    Guid Id,
    string Status,
    decimal Total,
    DateTime CreatedAt,
    List<OrderItemDto> Items
);

public record OrderItemDto(
    Guid ProductId,
    string ProductName,
    int Quantity,
    decimal UnitPrice,
    decimal Subtotal
);
