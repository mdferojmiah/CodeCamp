using Dukaan.Application.Dtos;
using Dukaan.Application.Features.Auth.Dtos;

namespace Dukaan.Application.Interfaces;

public interface IUserService
{
    Task<bool> CreateMarchantAsync(MerchantDto merchant, string password);
    Task<LoginResponseDto> LoginMarchantAsync(LoginRequestDto loginRequest);
}