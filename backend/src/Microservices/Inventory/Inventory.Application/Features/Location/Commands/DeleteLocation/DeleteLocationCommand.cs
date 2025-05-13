using Shared.Abstractions.Messaging.ResultType;

namespace Inventory.Application.Features.Location.Commands.DeleteLocation;

public record DeleteLocationCommand(Guid LocationId) : IResultCommand;
