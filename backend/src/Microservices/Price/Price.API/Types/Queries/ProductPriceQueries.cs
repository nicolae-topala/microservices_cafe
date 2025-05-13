using MediatR;
using Price.Application.Features.ProductPrice.Queries.GetProductPrices;
using Price.Domain.Entities;

namespace Price.API.Types.Queries;

[QueryType]
public class ProductPriceQueries
{
    [UsePaging]
    [UseFiltering]
    [UseSorting]
    public Task<IQueryable<ProductPrice>> GetProductPrices(ISender sender, CancellationToken cancellationToken) =>
        sender.Send(new GetProductPricesQuery(), cancellationToken);
}
