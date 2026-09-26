using Dukaan.Application.Features.Auth.Dtos;

namespace Dukaan.Application.Features.Auth.Services;

public interface IAuthService
{
    Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
}