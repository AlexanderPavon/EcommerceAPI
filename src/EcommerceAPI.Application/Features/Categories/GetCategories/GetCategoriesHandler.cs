using EcommerceAPI.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EcommerceAPI.Application.Features.Categories.GetCategories;

public class GetCategoriesHandler : IRequestHandler<GetCategoriesQuery, List<CategoryDto>>
{
    private readonly IApplicationDbContext _context;

    public GetCategoriesHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<CategoryDto>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        return await _context.Categories
            .Select(c => new CategoryDto(c.Id, c.Name, c.Description))
            .ToListAsync(cancellationToken);
    }
}
