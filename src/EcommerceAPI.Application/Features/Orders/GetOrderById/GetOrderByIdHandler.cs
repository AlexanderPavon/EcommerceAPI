using EcommerceAPI.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EcommerceAPI.Application.Features.Orders.GetOrderById;

public class GetOrderByIdHandler : IRequestHandler<GetOrderByIdQuery, OrderDetailDto?>
{
    private readonly IApplicationDbContext _context;

    public GetOrderByIdHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<OrderDetailDto?> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await _context.Orders
            .Include(o => o.Items)
            .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(o => o.Id == request.OrderId && o.UserId == request.UserId, cancellationToken);

        if (order is null) return null;

        return new OrderDetailDto(
            order.Id,
            order.Status.ToString(),
            order.Total,
            order.CreatedAt,
            order.Items.Select(i => new OrderItemDto(
                i.ProductId,
                i.Product.Name,
                i.Quantity,
                i.UnitPrice,
                i.UnitPrice * i.Quantity
            )).ToList()
        );
    }
}
