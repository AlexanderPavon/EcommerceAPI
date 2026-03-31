using Microsoft.AspNetCore.Identity;

namespace EcommerceAPI.Application.Common.Interfaces;

public interface IJwtService
{
    string GenerateToken(IdentityUser user, IList<string> roles);
}
