using MediatR;

namespace EcommerceAPI.Application.Features.Products.GetProducts;

public record GetProductsQuery(string? Search = null, Guid? CategoryId = null) : IRequest<List<ProductDto>>;

public record ProductDto(
    Guid Id,
    string Name,
    string? Description,
    decimal Price,
    int Stock,
    string? ImageUrl,
    string CategoryName
);
