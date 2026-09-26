using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Dukaan.Application.Dtos;
using Dukaan.Application.Features.Auth.Dtos;
using Dukaan.Application.Interfaces;
using Dukaan.Infrastructure.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Dukaan.Infrastructure.Data.Services;

public class UserService(IUserStore<Merchant> store,
    IOptions<IdentityOptions> optionsAccessor,
    IPasswordHasher<Merchant> passwordHasher,
    IEnumerable<IUserValidator<Merchant>> userValidators,
    IEnumerable<IPasswordValidator<Merchant>> passwordValidators,
    ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors,
    IServiceProvider services,
    Microsoft.Extensions.Logging.ILogger<UserManager<Merchant>> logger,
    IConfiguration config) 
    : UserManager<Merchant>(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger), IUserService
{

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

    public async Task<LoginResponseDto> LoginMarchantAsync(LoginRequestDto loginRequest)
    {
        var merchant = await FindByEmailAsync(loginRequest.Email);
        if(merchant is null) throw new UnauthorizedAccessException("Invalid credentials");

        var isValid = await CheckPasswordAsync(merchant, loginRequest.Password);
        if(!isValid) throw new UnauthorizedAccessException("Invalid credentials");

        var jwtToken = GenerateToken(merchant);
        var expiresAt = DateTime.UtcNow.AddMinutes(double.Parse(config["Jwt:ExpirationMinutes"]!));

        return new LoginResponseDto(jwtToken, expiresAt);
    }

    private string GenerateToken(Merchant merchant)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, merchant.Id.ToString()),
            new(ClaimTypes.Email, merchant.Email!),
            new("tenant_id", merchant.TenantId.ToString())
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(double.Parse(config["Jwt:ExpirationMinutes"]!)),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!)), 
                SecurityAlgorithms.HmacSha256)
        };

        var handler = new JwtSecurityTokenHandler();
        var securityToken = handler.CreateToken(tokenDescriptor);
        return handler.WriteToken(securityToken);
    }

}