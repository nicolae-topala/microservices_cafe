using Inventory.Application.Abstractions;
using Microsoft.EntityFrameworkCore;
using Shared.Abstractions.Messaging;
using ItemDomain = Inventory.Domain.Entities.Item;

namespace Inventory.Application.Features.Item.Queries.GetItems;

public class GetItemsQueryHandler(IInventoryDbContext dbContext) 
    : IQueryHandler<GetItemsQuery, IQueryable<ItemDomain>>
{
    public Task<IQueryable<ItemDomain>> Handle(GetItemsQuery request, CancellationToken cancellationToken) => 
        Task.FromResult(dbContext.Items
            .Include(i => i.Location)
                .ThenInclude(i => i.LocationType)
            .AsNoTracking());
}
