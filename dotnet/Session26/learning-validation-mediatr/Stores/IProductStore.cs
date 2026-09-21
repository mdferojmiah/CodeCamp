using learning_validation_mediatr.Models;

namespace learning_validation_mediatr.Stores;

public interface IProductStore
{
    Product Add(Product product);
    IEnumerable<Product> GetAll();
    Product? GetById(int id);
    Product? Update(int id, Product product);
    bool Delete(int id);
}