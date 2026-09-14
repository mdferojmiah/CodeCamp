namespace  Dukaan.Host.DTOs;


public record RegisterRequest(
    string Email,
    string PhoneNumber,
    string Password,
    string StoreName,
    string Slug,
    string Category,
    string Country
);