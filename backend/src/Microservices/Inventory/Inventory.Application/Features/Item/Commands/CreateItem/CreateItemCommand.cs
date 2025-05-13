using Inventory.Shared.DTOs.Item;
using Shared.Abstractions.Messaging.ResultType;
using ItemDomain = Inventory.Domain.Entities.Item;

namespace Inventory.Application.Features.Item.Commands.CreateItem;

public record CreateItemCommand(CreateItemDto Item) : IResultCommand<ItemDomain>;
