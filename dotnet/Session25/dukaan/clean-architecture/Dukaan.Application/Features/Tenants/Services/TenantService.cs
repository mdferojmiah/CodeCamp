using Dukaan.Application.Dtos;
using Dukaan.Application.Features.Tenants.Dtos;
using Dukaan.Application.Interfaces;
using Dukaan.Domain.Tenants;

namespace Dukaan.Application.Features.Tenants.Services;

public class TenantService(IRepository<Tenant> tenantRepository, IUserService userService): ITenantService
{
    public async Task<RegisterResponseDto> RegisterMerchantAsync(RegisterDto request)
    {
        var tenant = new Tenant
        {
            Id = Guid.NewGuid(),
            StoreName = request.StoreName,
            Slug = request.Slug,
            Category = request.Category,
            Country = request.Country
        };
        await tenantRepository.AddAsync(tenant);
        await tenantRepository.SaveChangesAsync();

        var merchant = new MerchantDto
        (
            request.Email,
            request.PhoneNumber,
            request.StoreName,
            request.Slug,
            request.Category,
            request.Country,
            tenant.Id
        );
        var result = await userService.CreateMarchantAsync(merchant, request.Password);

        if (!result)
        {
            throw new InvalidOperationException("Failed to create merchant!");
        }

        return new RegisterResponseDto(
            TenantId: tenant.Id.ToString(),
            StoreName: tenant.StoreName
        );
    }
}