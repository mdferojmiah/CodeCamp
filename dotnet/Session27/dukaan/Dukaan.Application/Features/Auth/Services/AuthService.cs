using Dukaan.Application.Features.Auth.Dtos;
using Dukaan.Application.Interfaces;

namespace Dukaan.Application.Features.Auth.Services;

public class AuthService(IUserService userService): IAuthService
{
    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
    {
        return await userService.LoginMarchantAsync(request);
    }
}