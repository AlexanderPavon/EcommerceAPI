using EcommerceAPI.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EcommerceAPI.Application.Features.Cart.RemoveFromCart;

public class RemoveFromCartHandler : IRequestHandler<RemoveFromCartCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public RemoveFromCartHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(RemoveFromCartCommand request, CancellationToken cancellationToken)
    {
        var cart = await _context.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.UserId == request.UserId, cancellationToken);

        if (cart is null) return false;

        var item = cart.Items.FirstOrDefault(i => i.Id == request.CartItemId);
        if (item is null) return false;

        cart.Items.Remove(item);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
