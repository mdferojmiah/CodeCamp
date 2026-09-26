using Dukaan.Application.Features.Products.Dtos;

namespace Dukaan.Application.Features.Products.Services;

public interface IProductService
{
    Task<ProductCreationResponseDto> CreateAsync(ProductCreationRequestDto request);
}