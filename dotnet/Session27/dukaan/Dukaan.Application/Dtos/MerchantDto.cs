namespace Dukaan.Application.Dtos;

public record MerchantDto(
    string Email,
    string PhoneNumber,
    string StoreName,
    string Slug,
    string Category,
    string Country,
    Guid TenantId
);