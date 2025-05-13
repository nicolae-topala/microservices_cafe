using Microsoft.EntityFrameworkCore;
using Price.Application.Abstractions;
using Shared.Abstractions.Messaging;
using ProductPriceDomain = Price.Domain.Entities.ProductPrice;

namespace Price.Application.Features.ProductPrice.Queries.GetProductPrices;

public class GetProductPricesQueryHandler(IPriceDbContext dbContext)
    : IQueryHandler<GetProductPricesQuery, IQueryable<ProductPriceDomain>>
{
    public Task<IQueryable<ProductPriceDomain>> Handle(GetProductPricesQuery request, CancellationToken cancellationToken) =>
        Task.FromResult(
            dbContext.ProductPrices
                .Include(dr => dr.Channel)
                .AsNoTracking());
}
