using learning_validation_mediatr.Services;
using learning_validation_mediatr.Stores;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSingleton<IProductStore, ProductStore>();
builder.Services.AddScoped<IProductService, ProductService>();


var app = builder.Build();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
