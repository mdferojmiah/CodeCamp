using Dukaan.Application.Dtos;
using Dukaan.Application.Interfaces;
using Dukaan.Infrastructure.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Dukaan.Infrastructure.Data.Services;

public class UserService : UserManager<Merchant>, IUserService
{
    public UserService(IUserStore<Merchant> store, 
        IOptions<IdentityOptions> optionsAccessor, 
        IPasswordHasher<Merchant> passwordHasher, 
        IEnumerable<IUserValidator<Merchant>> userValidators, 
        IEnumerable<IPasswordValidator<Merchant>> passwordValidators, 
        ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, 
        IServiceProvider services, 
        Microsoft.Extensions.Logging.ILogger<UserManager<Merchant>> logger) 
        : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
    {
    }

    public async Task<bool> CreateMarchantAsync(MerchantDto merchant, string password)
    {
        var merchantEo = new Merchant
        {
            UserName = merchant.Email,
            Email = merchant.Email,
            PhoneNumber = merchant.PhoneNumber,
            TenantId = merchant.TenantId,
            RegisterAt = DateTime.UtcNow
        };
        var result = await CreateAsync(merchantEo, password);
        return result.Succeeded;
    }
}