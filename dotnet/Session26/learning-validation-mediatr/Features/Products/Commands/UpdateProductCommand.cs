using FluentValidation;
using learning_validation_mediatr.Models;
using learning_validation_mediatr.Stores;
using MediatR;

namespace learning_validation_mediatr.Features.Products.Commands;

public record UpdateProductCommand(int Id, string Name, decimal Price) : IRequest<Product?>;

public class UpdateProductCommandValidator: AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name can not be empty")
            .MaximumLength(10).WithMessage("Name can not exceed 10 characters");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price can not be zero");
    }   
}

public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, Product?>
{
    private readonly IProductStore _store;
    public UpdateProductHandler(IProductStore store)
    {
        _store = store;
    }
    public Task<Product?> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var product = _store.Update(request.Id, new Product { Name = request.Name, Price = request.Price });
        return Task.FromResult(product is null ? null : product);
    }
}