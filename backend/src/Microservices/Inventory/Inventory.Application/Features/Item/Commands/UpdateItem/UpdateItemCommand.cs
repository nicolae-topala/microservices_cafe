using Inventory.Shared.DTOs.Item;
using Shared.Abstractions.Messaging.ResultType;

namespace Inventory.Application.Features.Item.Commands.UpdateItem;

public record UpdateItemCommand(UpdateItemDto Item) : IResultCommand;
