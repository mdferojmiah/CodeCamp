//Dependency Injection
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddOpenApi();
var app = builder.Build();

//Middleware
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseHttpsRedirection();


//Mapping endpoints
app.MapPost("/product", (HttpContext context,[FromBody] Product product) =>
{
    Console.WriteLine($"Product Name: {product.Name} | Price: {product.Price}");
    return Results.Ok("Product added successfully!");
});

app.MapGet("/hello", (string message) =>
{
    Console.WriteLine("Received message: " + message);
    return Results.Ok(new {Message = message});
})
.WithName("GetHello");

app.Run();

class Product
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
}
