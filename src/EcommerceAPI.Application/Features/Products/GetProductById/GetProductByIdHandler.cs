using EcommerceAPI.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EcommerceAPI.Application.Features.Products.GetProductById;

public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, ProductDetailDto?>
{
    private readonly IApplicationDbContext _context;

    public GetProductByIdHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ProductDetailDto?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.Products
            .Include(p => p.Category)
            .Where(p => p.Id == request.Id)
            .Select(p => new ProductDetailDto(
                p.Id, p.Name, p.Description, p.Price, p.Stock,
                p.ImageUrl, p.IsActive, p.CategoryId, p.Category.Name))
            .FirstOrDefaultAsync(cancellationToken);
    }
}
