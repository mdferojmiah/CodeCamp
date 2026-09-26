using Dukaan.Application.Features.Products.Dtos;
using Dukaan.Application.Features.Products.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Dukaan.Host.Controllers;

[Authorize]
[ApiController]
[Route("/api/[controller]")]
public class ProductsController(IProductService productService) : ControllerBase
{
    [HttpPost("create")]
    public async Task<IActionResult> CreateProductAsync(ProductCreationRequestDto request)
    {
        var response = await productService.CreateAsync(request);
        return Ok(response);
    }
}