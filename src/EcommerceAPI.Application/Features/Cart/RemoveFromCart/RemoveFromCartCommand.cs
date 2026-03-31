using MediatR;

namespace EcommerceAPI.Application.Features.Cart.RemoveFromCart;

public record RemoveFromCartCommand(string UserId, Guid CartItemId) : IRequest<bool>;
