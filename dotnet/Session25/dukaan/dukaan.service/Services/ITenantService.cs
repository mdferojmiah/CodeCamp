using dukaan.service.Dtos;

namespace dukaan.service.Services;

public interface ITenantService
{
    Task<RegisterResponseDto> RegisterMerchantAsync(RegisterDto request);
}