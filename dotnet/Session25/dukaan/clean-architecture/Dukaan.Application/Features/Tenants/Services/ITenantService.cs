using Dukaan.Application.Features.Tenants.Dtos;

namespace Dukaan.Application.Features.Tenants.Services;

public interface ITenantService
{
    Task<RegisterResponseDto> RegisterMerchantAsync(RegisterDto request);
}