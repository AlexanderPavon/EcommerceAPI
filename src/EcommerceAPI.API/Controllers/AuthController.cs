using EcommerceAPI.Application.Features.Auth.Login;
using EcommerceAPI.Application.Features.Auth.Register;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAPI.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterCommand command)
    {
        var result = await _mediator.Send(command);
        if (!result.Succeeded)
            return BadRequest(new { error = result.Error });

        return Ok(new { message = "User registered successfully" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand command)
    {
        var result = await _mediator.Send(command);
        if (!result.Succeeded)
            return Unauthorized(new { error = result.Error });

        return Ok(new { token = result.Token });
    }
}
