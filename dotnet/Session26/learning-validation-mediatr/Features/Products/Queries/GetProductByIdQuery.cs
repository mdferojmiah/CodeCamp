using learning_validation_mediatr.Models;
using learning_validation_mediatr.Stores;
using MediatR;

namespace learning_validation_mediatr.Features.Products.Queries;

public record GetProductByIdQuery(int Id) : IRequest<Product?>;

public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, Product?>
{
    private readonly IProductStore _store;
    public GetProductByIdHandler(IProductStore store)
    {
        _store = store;
    }
    public Task<Product?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = _store.GetById(request.Id);
        return Task.FromResult(product is null ? null : product);
    }
}