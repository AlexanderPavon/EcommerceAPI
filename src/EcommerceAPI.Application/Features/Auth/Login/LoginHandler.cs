using EcommerceAPI.Application.Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace EcommerceAPI.Application.Features.Auth.Login;

public class LoginHandler : IRequestHandler<LoginCommand, LoginResponse>
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IJwtService _jwtService;

    public LoginHandler(UserManager<IdentityUser> userManager, IJwtService jwtService)
    {
        _userManager = userManager;
        _jwtService = jwtService;
    }

    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
            return new LoginResponse(false, Error: "Invalid credentials");

        var validPassword = await _userManager.CheckPasswordAsync(user, request.Password);
        if (!validPassword)
            return new LoginResponse(false, Error: "Invalid credentials");

        var roles = await _userManager.GetRolesAsync(user);
        var token = _jwtService.GenerateToken(user, roles);

        return new LoginResponse(true, token);
    }
}
