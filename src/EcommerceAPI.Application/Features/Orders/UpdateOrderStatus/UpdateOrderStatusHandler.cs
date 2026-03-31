using EcommerceAPI.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EcommerceAPI.Application.Features.Orders.UpdateOrderStatus;

public class UpdateOrderStatusHandler : IRequestHandler<UpdateOrderStatusCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public UpdateOrderStatusHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
    {
        var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken);
        if (order is null) return false;

        order.Status = request.Status;
        order.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
