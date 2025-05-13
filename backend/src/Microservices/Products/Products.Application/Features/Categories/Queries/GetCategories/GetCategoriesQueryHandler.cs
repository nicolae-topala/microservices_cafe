using Microsoft.EntityFrameworkCore;
using Products.Application.Abstractions;
using Products.Domain.Entities;
using Shared.Abstractions.Messaging;

namespace Products.Application.Features.Categories.Queries.GetCategories;

public class GetCategoriesQueryHandler(IProductsDbContext dbContext)
    : IQueryHandler<GetCategoriesQuery, IQueryable<Category>>
{
    public Task<IQueryable<Category>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken) =>
        Task.FromResult(
            dbContext.Categories
                .Include(c => c.SubCategories)
                .Include(c => c.Products)
                .Include(c => c.ParentCategory)
                .AsNoTracking());
}