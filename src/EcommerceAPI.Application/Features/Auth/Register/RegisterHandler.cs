using MediatR;
using Microsoft.AspNetCore.Identity;

namespace EcommerceAPI.Application.Features.Auth.Register;

public class RegisterHandler : IRequestHandler<RegisterCommand, RegisterResponse>
{
    private readonly UserManager<IdentityUser> _userManager;

    public RegisterHandler(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<RegisterResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var user = new IdentityUser
        {
            Email = request.Email,
            UserName = request.Email
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            var error = string.Join(", ", result.Errors.Select(e => e.Description));
            return new RegisterResponse(false, error);
        }

        await _userManager.AddToRoleAsync(user, "Customer");

        return new RegisterResponse(true);
    }
}
