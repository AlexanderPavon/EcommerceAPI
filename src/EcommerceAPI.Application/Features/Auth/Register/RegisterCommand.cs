using MediatR;

namespace EcommerceAPI.Application.Features.Auth.Register;

public record RegisterCommand(
    string Email,
    string Password,
    string FirstName,
    string LastName
) : IRequest<RegisterResponse>;

public record RegisterResponse(bool Succeeded, string? Error = null);
