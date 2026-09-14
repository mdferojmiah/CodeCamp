using Dukaan.Application.Features.Tenants.Dtos;
using Dukaan.Application.Features.Tenants.Services;
using Dukaan.Host.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Dukaan.Host.Controllers;

[ApiController]
[Route("[controller]")]
public class TenantsController(ITenantService tenantService): ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult> Register(RegisterRequest request)
    {
        try
        {
            var result = await tenantService.RegisterMerchantAsync(new RegisterDto(
                Email: request.Email,
                PhoneNumber: request.PhoneNumber,
                Password: request.Password,
                StoreName: request.StoreName,
                Slug: request.Slug,
                Category: request.Category,
                Country: request.Country
            ));

            return Ok(result);
        }
        catch(Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}