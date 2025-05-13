using Shared.Abstractions.Messaging.ResultType;

namespace Inventory.Application.Features.Item.Commands.DeleteItem;

public record DeleteItemCommand(Guid ItemId) : IResultCommand;
