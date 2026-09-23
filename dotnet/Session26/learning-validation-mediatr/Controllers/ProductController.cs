using learning_validation_mediatr.Dtos;
using learning_validation_mediatr.Features.Products.Commands;
using learning_validation_mediatr.Features.Products.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace learning_validation_mediatr.Controllers;

[ApiController]
[Route("api/products")]
public class ProductController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductResponse>>> GetAll()
    {
        var products = await _mediator.Send(new GetAllProductsQuery());
        return Ok(products);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductResponse>> GetById(int id)
    {
        var product = await _mediator.Send(new GetProductByIdQuery(id));
        return product is null ? NotFound() : Ok(product);
    }

    [HttpPost]
    public async Task<ActionResult<ProductResponse>> Create(CreateProductCommand command)
    {
        var product = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ProductResponse>> Update(int id, UpdateProductRequest request)
    {
        var product = await _mediator.Send(new UpdateProductCommand(id, request.Name, request.Price));
        return product is null ? NotFound() : Ok(product);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult<ProductResponse>> Delete(int id)
    {
        var deleted = await _mediator.Send(new DeleteProductCommand(id));
        return deleted is null ? NotFound(): Ok(deleted);
    }
}