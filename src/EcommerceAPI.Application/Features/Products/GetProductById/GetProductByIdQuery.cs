using MediatR;

namespace EcommerceAPI.Application.Features.Products.GetProductById;

public record GetProductByIdQuery(Guid Id) : IRequest<ProductDetailDto?>;

public record ProductDetailDto(
    Guid Id,
    string Name,
    string? Description,
    decimal Price,
    int Stock,
    string? ImageUrl,
    bool IsActive,
    Guid CategoryId,
    string CategoryName
);
