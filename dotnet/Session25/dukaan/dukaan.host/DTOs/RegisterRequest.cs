namespace dukaan.host.Dtos;

public record RegisterRequest(
    string Email,
    string PhoneNumber,
    string Password,
    string StoreName,
    string Slug,
    string Category,
    string Country
);