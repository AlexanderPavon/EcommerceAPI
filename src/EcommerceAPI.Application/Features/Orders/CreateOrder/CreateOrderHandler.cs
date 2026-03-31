using EcommerceAPI.Application.Common.Interfaces;
using EcommerceAPI.Domain.Entities;
using EcommerceAPI.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EcommerceAPI.Application.Features.Orders.CreateOrder;

public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, CreateOrderResponse>
{
    private readonly IApplicationDbContext _context;

    public CreateOrderHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CreateOrderResponse> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var cart = await _context.Carts
            .Include(c => c.Items)
            .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(c => c.UserId == request.UserId, cancellationToken);

        if (cart is null || !cart.Items.Any())
            throw new InvalidOperationException("Cart is empty");

        var order = new Order
        {
            UserId = request.UserId,
            Status = OrderStatus.Pending,
            Items = cart.Items.Select(i => new OrderItem
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                UnitPrice = i.Product.Price
            }).ToList()
        };

        order.Total = order.Items.Sum(i => i.UnitPrice * i.Quantity);

        // Descontar stock
        foreach (var item in cart.Items)
        {
            item.Product.Stock -= item.Quantity;
            item.Product.UpdatedAt = DateTime.UtcNow;
        }

        _context.Orders.Add(order);
        _context.Carts.Remove(cart);

        await _context.SaveChangesAsync(cancellationToken);

        return new CreateOrderResponse(order.Id, order.Total);
    }
}
