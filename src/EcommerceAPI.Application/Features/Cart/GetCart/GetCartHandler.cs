using EcommerceAPI.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EcommerceAPI.Application.Features.Cart.GetCart;

public class GetCartHandler : IRequestHandler<GetCartQuery, CartDto?>
{
    private readonly IApplicationDbContext _context;

    public GetCartHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CartDto?> Handle(GetCartQuery request, CancellationToken cancellationToken)
    {
        var cart = await _context.Carts
            .Include(c => c.Items)
            .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(c => c.UserId == request.UserId, cancellationToken);

        if (cart is null) return null;

        var items = cart.Items.Select(i => new CartItemDto(
            i.Id,
            i.ProductId,
            i.Product.Name,
            i.Product.Price,
            i.Quantity,
            i.Product.Price * i.Quantity
        )).ToList();

        return new CartDto(cart.Id, cart.UserId, items, items.Sum(i => i.Subtotal));
    }
}
