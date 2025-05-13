using HotChocolate.Authorization;
using MediatR;
using Price.Application.Features.ProductPrice.Commands.CreateProductPrice;
using Price.Application.Features.ProductPrice.Commands.DeleteProductPrice;
using Price.Application.Features.ProductPrice.Commands.UpdateProductPrice;
using Price.Domain.Entities;
using Price.Shared.DTOs.ProductPrice;
using Shared.BuildingBlocks.Result;
using Shared.Helpers.Hotchocolate;

namespace Price.API.Types.Mutations;

[MutationType]
[Authorize]
public class ProductPriceMutations
{
    [Error<ResultError>]
    public async Task<FieldResult<ProductPrice>> CreateProductPrice(ISender sender, CreateProductPriceDto productPriceDto, CancellationToken cancellationToken) =>
        ResultHandler.HandleResponse(await sender.Send(new CreateProductPriceCommand(productPriceDto), cancellationToken));

    [Error<ResultError>]
    public async Task<FieldResult<bool>> UpdateProductPrice(ISender sender, UpdateProductPriceDto productPriceDto, CancellationToken cancellationToken) =>
        ResultHandler.HandleResponse(await sender.Send(new UpdateProductPriceCommand(productPriceDto), cancellationToken));

    [Error<ResultError>]
    public async Task<FieldResult<bool>> DeleteProductPrice(ISender sender, Guid id, CancellationToken cancellationToken) =>
        ResultHandler.HandleResponse(await sender.Send(new DeleteProductPriceCommand(id), cancellationToken));
}
