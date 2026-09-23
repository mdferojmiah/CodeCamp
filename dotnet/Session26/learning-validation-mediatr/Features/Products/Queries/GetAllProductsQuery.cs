using learning_validation_mediatr.Models;
using learning_validation_mediatr.Stores;
using MediatR;

namespace learning_validation_mediatr.Features.Products.Queries;

public record GetAllProductsQuery: IRequest<IEnumerable<Product>>;

public class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, IEnumerable<Product>>
{
    private readonly IProductStore _store;
    public GetAllProductsHandler(IProductStore store)
    {
        _store = store;
    }
    public async Task<IEnumerable<Product>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        var responses = _store.GetAll();
        return await Task.FromResult(responses);
    }
}