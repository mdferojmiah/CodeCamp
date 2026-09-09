using dukaan.host.Dtos;
using dukaan.service.Dtos;
using dukaan.service.Services;
using Microsoft.AspNetCore.Mvc;

namespace dukaan.host.Controllers;

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