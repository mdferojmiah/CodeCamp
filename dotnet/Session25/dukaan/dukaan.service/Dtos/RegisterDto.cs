namespace dukaan.service.Dtos;

public record RegisterDto(
    string Email,
    string PhoneNumber,
    string Password,
    string StoreName,
    string Slug,
    string Category,
    string Country
);

public record RegisterResponseDto(
    string TenantId,
    string StoreName
);