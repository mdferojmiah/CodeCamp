using FluentValidation;
using learning_validation_mediatr.Models;
using learning_validation_mediatr.Stores;
using MediatR;

namespace learning_validation_mediatr.Features.Products.Commands;

public record CreateProductCommand(string Name, decimal Price): IRequest<Product>;

public class CreateProductCommandValidator: AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("[Fluentvalidation] Name can not be empty!")
            .MaximumLength(10).WithMessage("[Fluentvalidation] name can not exceed 10 characters");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("[Fluentvalidation] Price can not be zero.");
    }
}

public class CreateProductHandler : IRequestHandler<CreateProductCommand, Product>
{
    private readonly IProductStore _store;
    public CreateProductHandler(IProductStore store)
    {
        _store = store;
    }
    public Task<Product> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = _store.Add(new Product { Name = request.Name, Price = request.Price });
        return Task.FromResult(product);
    }
}