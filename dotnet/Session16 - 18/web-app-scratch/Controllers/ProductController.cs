using web_app_scratch.Attributes;

namespace web_app_scratch.Controllers;


public record CreateProductRequest(string Name, string Price);
public class ProductController
{
    [HttpGet("/products")]
    public string GetAll()
    {
        return "All Product list";
    }

    [HttpPost("/products")]
    public string Create(CreateProductRequest request)
    {
        return $"Returning product: {request.Name}: {request.Price}";
    }
}