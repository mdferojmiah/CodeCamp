using dukaan.service.Dtos;
using dukaan.service.Entities;
using dukaan.service.Repositories;
using Microsoft.AspNetCore.Identity;

namespace dukaan.service.Services;

public class TenantService(IRepository<Tenant> tenantRepository, UserManager<Merchant> userManager): ITenantService
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

        var merchant = new Merchant
        {
            UserName = request.Email,
            Email = request.Email,
            PhoneNumber = request.PhoneNumber,
            TenantId = tenant.Id
        };
        var result = await userManager.CreateAsync(merchant, request.Password);

        if (!result.Succeeded)
        {
            throw new InvalidOperationException("Failed to create merchant!");
        }

        return new RegisterResponseDto(
            TenantId: tenant.Id.ToString(),
            StoreName: tenant.StoreName
        );
    }
}