using MediatR;

namespace EcommerceAPI.Application.Features.Products.UpdateProduct;

public record UpdateProductCommand(
    Guid Id,
    string Name,
    string? Description,
    decimal Price,
    int Stock,
    string? ImageUrl,
    bool IsActive,
    Guid CategoryId
) : IRequest<bool>;
