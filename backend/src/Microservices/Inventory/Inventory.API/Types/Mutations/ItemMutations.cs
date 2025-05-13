using HotChocolate.Authorization;
using Inventory.Application.Features.Item.Commands.CreateItem;
using Inventory.Application.Features.Item.Commands.DeleteItem;
using Inventory.Application.Features.Item.Commands.UpdateItem;
using Inventory.Domain.Entities;
using Inventory.Shared.DTOs.Item;
using MediatR;
using Shared.BuildingBlocks.Result;
using Shared.Helpers.Hotchocolate;

namespace Inventory.API.Types.Mutations;

[MutationType]
[Authorize]
public class ItemMutations
{
    [Error<ResultError>]
    public async Task<FieldResult<Item>> CreateItem(ISender sender, CreateItemDto itemDto, CancellationToken cancellationToken) =>
        ResultHandler.HandleResponse(await sender.Send(new CreateItemCommand(itemDto), cancellationToken));

    [Error<ResultError>]
    public async Task<FieldResult<bool>> UpdateItem(ISender sender, UpdateItemDto itemDto, CancellationToken cancellationToken) =>
        ResultHandler.HandleResponse(await sender.Send(new UpdateItemCommand(itemDto), cancellationToken));

    [Error<ResultError>]
    public async Task<FieldResult<bool>> DeleteItem(ISender sender, Guid id, CancellationToken cancellationToken) =>
        ResultHandler.HandleResponse(await sender.Send(new DeleteItemCommand(id), cancellationToken));
}