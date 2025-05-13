using Shared.Abstractions.Messaging;
using ProductPriceDomain = Price.Domain.Entities.ProductPrice;

namespace Price.Application.Features.ProductPrice.Queries.GetProductPrices;

public record GetProductPricesQuery : IQuery<IQueryable<ProductPriceDomain>>;
