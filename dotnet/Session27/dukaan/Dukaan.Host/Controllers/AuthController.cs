using Dukaan.Application.Features.Auth.Dtos;
using Dukaan.Application.Features.Auth.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dukaan.Host.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> LoginWithJwt(LoginRequestDto request)
    {
        try
        {
            var result = await authService.LoginAsync(request);

            return Ok(result);
        }
        catch(UnauthorizedAccessException ex)
        {
            return Unauthorized(ex.Message);
        }
    }
}