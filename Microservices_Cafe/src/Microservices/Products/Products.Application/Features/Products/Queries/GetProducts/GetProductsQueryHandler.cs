using Microsoft.EntityFrameworkCore;
using Products.Application.Abstractions;
using Products.Domain.Entities;
using Shared.Abstractions.Messaging;
using Shared.BuildingBlocks.Result;

namespace Products.Application.Features.Products.Queries.GetProducts;
public class GetProductsQueryHandler(IProductsDbContext dbContext)
    : IQueryHandler<GetProductsQuery, IQueryable<Product>>
{
    public async Task<Result<IQueryable<Product>>> Handle(GetProductsQuery request, CancellationToken cancellationToken) =>
        Result.Create(dbContext.Products.AsNoTracking());
}
