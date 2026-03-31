using MediatR;

namespace EcommerceAPI.Application.Features.Cart.AddToCart;

public record AddToCartCommand(
    string UserId,
    Guid ProductId,
    int Quantity
) : IRequest<bool>;
