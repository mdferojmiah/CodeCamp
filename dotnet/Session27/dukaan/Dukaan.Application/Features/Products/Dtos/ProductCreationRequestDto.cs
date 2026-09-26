namespace Dukaan.Application.Features.Products.Dtos;

public record ProductCreationRequestDto(
    string Name,
    string? Description,
    decimal Price,
    string? ImageUrl,
    int StockQuantity);