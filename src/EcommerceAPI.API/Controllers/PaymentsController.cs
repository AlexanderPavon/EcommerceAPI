using System.Security.Claims;
using EcommerceAPI.Application.Features.Payments.CreatePaymentIntent;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Stripe;

namespace EcommerceAPI.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PaymentsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IConfiguration _configuration;

    public PaymentsController(IMediator mediator, IConfiguration configuration)
    {
        _mediator = mediator;
        _configuration = configuration;
    }

    private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    [HttpPost("create-intent/{orderId}")]
    [Authorize]
    public async Task<IActionResult> CreatePaymentIntent(Guid orderId)
    {
        try
        {
            var result = await _mediator.Send(new CreatePaymentIntentCommand(orderId, GetUserId()));
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPost("webhook")]
    public async Task<IActionResult> Webhook()
    {
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

        try
        {
            var stripeEvent = EventUtility.ParseEvent(json);

            if (stripeEvent.Type == EventTypes.PaymentIntentSucceeded)
            {
                var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                // Aquí actualizarías la orden a "Paid" usando el PaymentIntentId
                // Por ahora solo confirmamos que recibimos el webhook
            }

            return Ok();
        }
        catch
        {
            return BadRequest();
        }
    }
}
