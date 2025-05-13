using Shared.Abstractions.Messaging.ResultType;

namespace Inventory.Application.Features.Movement.Commands.DeleteMovement;

public record DeleteMovementCommand(Guid MovementId) : IResultCommand;
