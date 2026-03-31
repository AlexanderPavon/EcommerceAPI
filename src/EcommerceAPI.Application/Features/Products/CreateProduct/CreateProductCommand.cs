using MediatR;

namespace EcommerceAPI.Application.Features.Products.CreateProduct;

public record CreateProductCommand(
    string Name,
    string? Description,
    decimal Price,
    int Stock,
    string? ImageUrl,
    Guid CategoryId
) : IRequest<CreateProductResponse>;

public record CreateProductResponse(Guid Id, string Name, decimal Price, int Stock);
