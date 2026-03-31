using MediatR;

namespace EcommerceAPI.Application.Features.Cart.GetCart;

public record GetCartQuery(string UserId) : IRequest<CartDto?>;

public record CartDto(
    Guid Id,
    string UserId,
    List<CartItemDto> Items,
    decimal Total
);

public record CartItemDto(
    Guid Id,
    Guid ProductId,
    string ProductName,
    decimal ProductPrice,
    int Quantity,
    decimal Subtotal
);
