using Dukaan.Application.Features.Products.Dtos;
using Dukaan.Application.Interfaces;
using Dukaan.Domain.Products;

namespace Dukaan.Application.Features.Products.Services;

public class ProductService(IRepository<Product> productRepository): IProductService
{
    public async Task<ProductCreationResponseDto> CreateAsync(ProductCreationRequestDto request)
    {
        var product = new Product
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            ImageUrl = request.ImageUrl,
            StockQuantity = request.StockQuantity
        };

        await productRepository.AddAsync(product);
        await productRepository.SaveChangesAsync();

        return new ProductCreationResponseDto(
            product.Id, 
            product.Name, 
            product.Description, 
            product.Price, 
            product.ImageUrl, 
            product.StockQuantity,
            product.IsActive, 
            []);
    }
}