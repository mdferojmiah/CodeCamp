using learning_validation_mediatr.Models;

namespace learning_validation_mediatr.Stores;

public class ProductStore : IProductStore
{
    private readonly object _lock = new();
    private readonly Dictionary<int, Product> _products = new();
    private int _nextId = 1;

    public ProductStore()
    {
        Add(new Product { Name = "Laptop", Price = 999.99m });
        Add(new Product { Name = "Mouse", Price = 29.99m });
        Add(new Product { Name = "Keyboard", Price = 49.99m });
    }

    public Product Add(Product product)
    {
        lock (_lock)
        {
            product.Id = _nextId++;
            _products[product.Id] = product;
            return product;
        }
    }

    public IEnumerable<Product> GetAll()
    {
        lock (_lock)
        {
            return _products.Values.ToList();
        }
    }

    public Product? GetById(int id)
    {
        lock (_lock)
        {
            return _products.TryGetValue(id, out var product) ? product : null;
        }
    }

    public Product? Update(int id, Product product)
    {
        lock (_lock)
        {
            if (!_products.TryGetValue(id, out var existing))
            {
                return null;
            }

            existing.Name = product.Name;
            existing.Price = product.Price;
            return existing;
        }
    }

    public bool Delete(int id)
    {
        lock (_lock)
        {
            return _products.Remove(id);
        }
    }
}