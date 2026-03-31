using System.Security.Claims;
using EcommerceAPI.Application.Features.Cart.AddToCart;
using EcommerceAPI.Application.Features.Cart.GetCart;
using EcommerceAPI.Application.Features.Cart.RemoveFromCart;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAPI.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CartController : ControllerBase
{
    private readonly IMediator _mediator;

    public CartController(IMediator mediator)
    {
        _mediator = mediator;
    }

    private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    [HttpGet]
    public async Task<IActionResult> GetCart()
    {
        var result = await _mediator.Send(new GetCartQuery(GetUserId()));
        if (result is null) return Ok(new { items = new List<object>(), total = 0 });
        return Ok(result);
    }

    [HttpPost("items")]
    public async Task<IActionResult> AddToCart([FromBody] AddToCartRequest request)
    {
        var result = await _mediator.Send(new AddToCartCommand(GetUserId(), request.ProductId, request.Quantity));
        if (!result) return BadRequest(new { error = "Product not found or insufficient stock" });
        return Ok(new { message = "Item added to cart" });
    }

    [HttpDelete("items/{cartItemId}")]
    public async Task<IActionResult> RemoveFromCart(Guid cartItemId)
    {
        var result = await _mediator.Send(new RemoveFromCartCommand(GetUserId(), cartItemId));
        if (!result) return NotFound();
        return NoContent();
    }
}

public record AddToCartRequest(Guid ProductId, int Quantity);
