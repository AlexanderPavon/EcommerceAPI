using EcommerceAPI.Application.Common.Interfaces;
using EcommerceAPI.Domain.Entities;
using MediatR;

namespace EcommerceAPI.Application.Features.Categories.CreateCategory;

public class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, CreateCategoryResponse>
{
    private readonly IApplicationDbContext _context;

    public CreateCategoryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CreateCategoryResponse> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = new Category
        {
            Name = request.Name,
            Description = request.Description
        };

        _context.Categories.Add(category);
        await _context.SaveChangesAsync(cancellationToken);

        return new CreateCategoryResponse(category.Id, category.Name);
    }
}
