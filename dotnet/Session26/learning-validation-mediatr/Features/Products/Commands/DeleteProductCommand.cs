using learning_validation_mediatr.Models;
using learning_validation_mediatr.Stores;
using MediatR;

namespace learning_validation_mediatr.Features.Products.Commands;

public record DeleteProductCommand(int Id) : IRequest<Product?>;

public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, Product?>
{
    private readonly IProductStore _store;
    public DeleteProductHandler(IProductStore store)
    {
        _store = store;
    }
    public Task<Product?> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var storedProduct = _store.GetById(request.Id);
        if (storedProduct is null)
        {
            return Task.FromResult<Product?>(null);
        }
        var isDeleted = _store.Delete(request.Id);
        return Task.FromResult(isDeleted ? storedProduct : null);

    }
}