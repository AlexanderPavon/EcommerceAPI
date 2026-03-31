using EcommerceAPI.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EcommerceAPI.Application.Features.Products.DeleteProduct;

public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public DeleteProductHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);
        if (product is null) return false;

        product.IsActive = false;
        product.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
