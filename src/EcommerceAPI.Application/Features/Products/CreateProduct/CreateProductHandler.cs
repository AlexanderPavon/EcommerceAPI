using EcommerceAPI.Application.Common.Interfaces;
using EcommerceAPI.Domain.Entities;
using MediatR;

namespace EcommerceAPI.Application.Features.Products.CreateProduct;

public class CreateProductHandler : IRequestHandler<CreateProductCommand, CreateProductResponse>
{
    private readonly IApplicationDbContext _context;

    public CreateProductHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CreateProductResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = new Product
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            Stock = request.Stock,
            ImageUrl = request.ImageUrl,
            CategoryId = request.CategoryId
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync(cancellationToken);

        return new CreateProductResponse(product.Id, product.Name, product.Price, product.Stock);
    }
}
