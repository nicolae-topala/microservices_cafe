using Inventory.Shared.DTOs.Movement;
using Shared.Abstractions.Messaging.ResultType;
using MovementDomain = Inventory.Domain.Entities.Movement;

namespace Inventory.Application.Features.Movement.Commands.CreateMovement;

public record CreateMovementCommand(CreateMovementDto Movement) : IResultCommand<MovementDomain>;
