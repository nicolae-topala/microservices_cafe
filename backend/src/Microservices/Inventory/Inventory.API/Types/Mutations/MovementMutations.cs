using HotChocolate.Authorization;
using Inventory.Application.Features.Movement.Commands.CreateMovement;
using Inventory.Application.Features.Movement.Commands.DeleteMovement;
using Inventory.Application.Features.Movement.Commands.UpdateMovement;
using Inventory.Domain.Entities;
using Inventory.Shared.DTOs.Movement;
using MediatR;
using Shared.BuildingBlocks.Result;
using Shared.Helpers.Hotchocolate;

namespace Inventory.API.Types.Mutations;

[MutationType]
[Authorize]
public class MovementMutations
{
    [Error<ResultError>]
    public async Task<FieldResult<Movement>> CreateMovement(ISender sender, CreateMovementDto movementDto, CancellationToken cancellationToken) =>
        ResultHandler.HandleResponse(await sender.Send(new CreateMovementCommand(movementDto), cancellationToken));

    [Error<ResultError>]
    public async Task<FieldResult<bool>> UpdateMovement(ISender sender, UpdateMovementDto movementDto, CancellationToken cancellationToken) =>
        ResultHandler.HandleResponse(await sender.Send(new UpdateMovementCommand(movementDto), cancellationToken));

    [Error<ResultError>]
    public async Task<FieldResult<bool>> DeleteMovement(ISender sender, Guid id, CancellationToken cancellationToken) =>
        ResultHandler.HandleResponse(await sender.Send(new DeleteMovementCommand(id), cancellationToken));
}
