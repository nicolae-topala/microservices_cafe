using Inventory.Application.Features.Item.Queries.GetItems;
using Inventory.Domain.Entities;
using MediatR;

namespace Inventory.API.Types.Queries;

[QueryType]
public class ItemQueries
{
    [UsePaging]
    [UseFiltering]
    [UseSorting]
    public Task<IQueryable<Item>> GetItems(ISender sender, CancellationToken cancellationToken) => 
        sender.Send(new GetItemsQuery(), cancellationToken);
}