using EcommerceAPI.Application.Features.Products.CreateProduct;
using EcommerceAPI.Application.Features.Products.DeleteProduct;
using EcommerceAPI.Application.Features.Products.GetProductById;
using EcommerceAPI.Application.Features.Products.GetProducts;
using EcommerceAPI.Application.Features.Products.UpdateProduct;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceAPI.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? search, [FromQuery] Guid? categoryId)
    {
        var result = await _mediator.Send(new GetProductsQuery(search, categoryId));
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetProductByIdQuery(id));
        if (result is null) return NotFound();
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateProductCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductCommand command)
    {
        if (id != command.Id) return BadRequest();
        var result = await _mediator.Send(command);
        if (!result) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _mediator.Send(new DeleteProductCommand(id));
        if (!result) return NotFound();
        return NoContent();
    }
}
