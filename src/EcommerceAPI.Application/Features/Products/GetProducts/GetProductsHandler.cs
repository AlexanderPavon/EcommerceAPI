using EcommerceAPI.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EcommerceAPI.Application.Features.Products.GetProducts;

public class GetProductsHandler : IRequestHandler<GetProductsQuery, List<ProductDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICacheService _cache;

    public GetProductsHandler(IApplicationDbContext context, ICacheService cache)
    {
        _context = context;
        _cache = cache;
    }

    public async Task<List<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"products_{request.Search}_{request.CategoryId}";

        var cached = await _cache.GetAsync<List<ProductDto>>(cacheKey);
        if (cached is not null) return cached;

        var query = _context.Products
            .Include(p => p.Category)
            .Where(p => p.IsActive);

        if (!string.IsNullOrWhiteSpace(request.Search))
            query = query.Where(p => p.Name.Contains(request.Search));

        if (request.CategoryId.HasValue)
            query = query.Where(p => p.CategoryId == request.CategoryId);

        var products = await query.Select(p => new ProductDto(
            p.Id,
            p.Name,
            p.Description,
            p.Price,
            p.Stock,
            p.ImageUrl,
            p.Category.Name
        )).ToListAsync(cancellationToken);

        await _cache.SetAsync(cacheKey, products, TimeSpan.FromMinutes(10));

        return products;
    }
}
