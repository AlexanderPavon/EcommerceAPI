using MediatR;

namespace EcommerceAPI.Application.Features.Products.DeleteProduct;

public record DeleteProductCommand(Guid Id) : IRequest<bool>;
