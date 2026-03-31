using EcommerceAPI.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EcommerceAPI.Application.Features.Orders.GetOrders;

public class GetOrdersHandler : IRequestHandler<GetOrdersQuery, List<OrderSummaryDto>>
{
    private readonly IApplicationDbContext _context;

    public GetOrdersHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<OrderSummaryDto>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        return await _context.Orders
            .Include(o => o.Items)
            .Where(o => o.UserId == request.UserId)
            .OrderByDescending(o => o.CreatedAt)
            .Select(o => new OrderSummaryDto(
                o.Id,
                o.Status.ToString(),
                o.Total,
                o.Items.Count,
                o.CreatedAt))
            .ToListAsync(cancellationToken);
    }
}
