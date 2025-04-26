using MediatR;
using Products.Application.Features.Categories.Queries.GetCategories;
using Products.Domain.Entities;

namespace Products.API.Types.Queries;

[QueryType]
public class CategoriesQueries
{
    [UsePaging]
    [UseFiltering]
    [UseSorting]
    public Task<IQueryable<Category>> GetCategories([Service] ISender sender, CancellationToken cancellationToken) =>
        sender.Send(new GetCategoriesQuery(), cancellationToken);
}
