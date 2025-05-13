using Shared.Abstractions.Messaging;
using ItemDomain = Inventory.Domain.Entities.Item;

namespace Inventory.Application.Features.Item.Queries.GetItems;

public record GetItemsQuery : IQuery<IQueryable<ItemDomain>>;
