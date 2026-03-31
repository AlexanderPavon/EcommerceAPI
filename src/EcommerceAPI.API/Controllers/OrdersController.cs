using System.Security.Claims;
using EcommerceAPI.Application.Features.Orders.CreateOrder;
using EcommerceAPI.Application.Features.Orders.GetOrderById;
using EcommerceAPI.Application.Features.Orders.GetOrders;
using EcommerceAPI.Application.Features.Orders.UpdateOrderStatus;
using EcommerceAPI.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAPI.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrdersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    [HttpGet]
    public async Task<IActionResult> GetOrders()
    {
        var result = await _mediator.Send(new GetOrdersQuery(GetUserId()));
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetOrderByIdQuery(id, GetUserId()));
        if (result is null) return NotFound();
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder()
    {
        try
        {
            var result = await _mediator.Send(new CreateOrderCommand(GetUserId()));
            return CreatedAtAction(nameof(GetById), new { id = result.OrderId }, result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPut("{id}/status")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateStatusRequest request)
    {
        var result = await _mediator.Send(new UpdateOrderStatusCommand(id, request.Status));
        if (!result) return NotFound();
        return NoContent();
    }
}

public record UpdateStatusRequest(OrderStatus Status);
