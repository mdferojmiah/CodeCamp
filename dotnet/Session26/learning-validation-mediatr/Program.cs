using FluentValidation;
using learning_validation_mediatr;
using learning_validation_mediatr.Middlewares;
using learning_validation_mediatr.Stores;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddSingleton<IProductStore, ProductStore>();

// builder.Services.AddFluentValidationAutoValidation(
//     config => config.DisableDataAnnotationsValidation = true);
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

builder.Services.AddMediatR(config =>
{
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.RegisterServicesFromAssemblyContaining<Program>();
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseMiddleware<GlobalExceptionHandler>();
app.MapControllers();

app.Run();
