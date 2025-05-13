using Microsoft.EntityFrameworkCore;
using Products.Application.Abstractions;
using Products.Domain.Entities;
using Shared.Abstractions.Messaging;

namespace Products.Application.Features.Products.Queries.GetProducts;

public class GetProductsQueryHandler(IProductsDbContext dbContext)
    : IQueryHandler<GetProductsQuery, IQueryable<Product>>
{
    public Task<IQueryable<Product>> Handle(GetProductsQuery request, CancellationToken cancellationToken) =>
        Task.FromResult(
            dbContext.Products
                .Include(p => p.Categories)
                .Include(p => p.Variants)
                    .ThenInclude(pv => pv.VariantAttributes)
                        .ThenInclude(pva => pva.AttributeDefinition)
                .Include(p => p.Variants)
                    .ThenInclude(pv => pv.VariantAttributes)
                        .ThenInclude(pv => pv.UnitsOfMeasure)
                .Include(p => p.Variants)
                    .ThenInclude(pv => pv.Images)
                .AsNoTracking());
}
