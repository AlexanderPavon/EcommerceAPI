using MediatR;

namespace EcommerceAPI.Application.Features.Categories.GetCategories;

public record GetCategoriesQuery : IRequest<List<CategoryDto>>;

public record CategoryDto(Guid Id, string Name, string? Description);
