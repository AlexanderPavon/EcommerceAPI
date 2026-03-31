using MediatR;

namespace EcommerceAPI.Application.Features.Categories.CreateCategory;

public record CreateCategoryCommand(
    string Name,
    string? Description
) : IRequest<CreateCategoryResponse>;

public record CreateCategoryResponse(Guid Id, string Name);
