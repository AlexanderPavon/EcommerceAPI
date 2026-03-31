using EcommerceAPI.Application.Common.Interfaces;
using EcommerceAPI.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EcommerceAPI.Application.Features.Cart.AddToCart;

public class AddToCartHandler : IRequestHandler<AddToCartCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public AddToCartHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(AddToCartCommand request, CancellationToken cancellationToken)
    {
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == request.ProductId, cancellationToken);
        if (product is null || product.Stock < request.Quantity) return false;

        var cart = await _context.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.UserId == request.UserId, cancellationToken);

        if (cart is null)
        {
            cart = new Domain.Entities.Cart { UserId = request.UserId };
            _context.Carts.Add(cart);
        }

        var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == request.ProductId);
        if (existingItem is not null)
            existingItem.Quantity += request.Quantity;
        else
            cart.Items.Add(new CartItem { CartId = cart.Id, ProductId = request.ProductId, Quantity = request.Quantity });

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
