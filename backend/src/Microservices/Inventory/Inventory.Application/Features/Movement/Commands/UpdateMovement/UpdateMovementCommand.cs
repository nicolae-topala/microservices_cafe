using Inventory.Shared.DTOs.Movement;
using Shared.Abstractions.Messaging.ResultType;

namespace Inventory.Application.Features.Movement.Commands.UpdateMovement;

public record UpdateMovementCommand(UpdateMovementDto Movement) : IResultCommand;
