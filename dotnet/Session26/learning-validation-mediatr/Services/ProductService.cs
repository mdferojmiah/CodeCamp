using learning_validation_mediatr.Dtos;
using learning_validation_mediatr.Models;
using learning_validation_mediatr.Stores;

namespace learning_validation_mediatr.Services;

public class ProductService : IProductService
{
    private readonly IProductStore _store;

    public ProductService(IProductStore store)
    {
        _store = store;
    }

    public Task<IEnumerable<ProductResponse>> GetAllAsync()
    {
        var responses = _store.GetAll().Select(ToResponse);
        return Task.FromResult(responses);
    }

    public Task<ProductResponse?> GetByIdAsync(int id)
    {
        var product = _store.GetById(id);
        return Task.FromResult(product is null ? null : ToResponse(product));
    }

    public Task<ProductResponse> CreateAsync(CreateProductRequest request)
    {
        var product = _store.Add(new Product { Name = request.Name, Price = request.Price });
        return Task.FromResult(ToResponse(product));
    }

    public Task<ProductResponse?> UpdateAsync(int id, UpdateProductRequest request)
    {
        var product = _store.Update(id, new Product { Name = request.Name, Price = request.Price });
        return Task.FromResult(product is null ? null : ToResponse(product));
    }

    public Task<bool> DeleteAsync(int id)
    {
        return Task.FromResult(_store.Delete(id));
    }

    private static ProductResponse ToResponse(Product product)
    {
        return new ProductResponse
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price
        };
    }
}