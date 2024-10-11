using Microsoft.EntityFrameworkCore;
using Products.Application.Abstractions;
using Products.Domain.Entities;
using Shared.Abstractions.Messaging;
using Shared.BuildingBlocks.Result;

namespace Products.Application.Features.Categories.Queries.GetCategories;

public class GetCategoriesQueryHandler(IProductsDbContext dbContext)
    : IQueryHandler<GetCategoriesQuery, IQueryable<Category>>
{
    public async Task<Result<IQueryable<Category>>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken) =>
        Result.Create(dbContext.Categories.AsNoTracking());
}