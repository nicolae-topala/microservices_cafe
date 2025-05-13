using HotChocolate.Authorization;
using Inventory.Application.Features.Location.Commands.CreateLocation;
using Inventory.Application.Features.Location.Commands.DeleteLocation;
using Inventory.Application.Features.Location.Commands.UpdateLocation;
using Inventory.Shared.DTOs.Location;
using MediatR;
using Shared.BuildingBlocks.Result;
using Shared.Helpers.Hotchocolate;
using Location = Inventory.Domain.Entities.Location;

namespace Inventory.API.Types.Mutations;

[MutationType]
[Authorize]
public class LocationMutations
{
    [Error<ResultError>]
    public async Task<FieldResult<Location>> CreateLocation(ISender sender, CreateLocationDto locationDto, CancellationToken cancellationToken) =>
        ResultHandler.HandleResponse(await sender.Send(new CreateLocationCommand(locationDto), cancellationToken));

    [Error<ResultError>]
    public async Task<FieldResult<bool>> UpdateLocation(ISender sender, UpdateLocationDto locationDto, CancellationToken cancellationToken) =>
        ResultHandler.HandleResponse(await sender.Send(new UpdateLocationCommand(locationDto), cancellationToken));

    [Error<ResultError>]
    public async Task<FieldResult<bool>> DeleteLocation(ISender sender, Guid id, CancellationToken cancellationToken) =>
        ResultHandler.HandleResponse(await sender.Send(new DeleteLocationCommand(id), cancellationToken));
}
