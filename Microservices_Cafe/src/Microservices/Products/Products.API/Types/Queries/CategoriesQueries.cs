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
    public async Task<IQueryable<Category>> GetCategories([Service] ISender sender)
    {
        var result = await sender.Send(new GetCategoriesQuery());
        return result.Value;
    }
}
