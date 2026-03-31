using MediatR;

namespace EcommerceAPI.Application.Features.Auth.Login;

public record LoginCommand(
    string Email,
    string Password
) : IRequest<LoginResponse>;

public record LoginResponse(bool Succeeded, string? Token = null, string? Error = null);
