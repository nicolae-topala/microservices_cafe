using HotChocolate.Authorization;
using MediatR;
using Products.Application.Features.Products.Commands.AddProductVariant;
using Products.Application.Features.Products.Commands.CreateProduct;
using Products.Application.Features.Products.Commands.DeleteProduct;
using Products.Application.Features.Products.Commands.EditProduct;
using Products.Application.Features.ProductVariants.Commands;
using Products.Domain.Entities;
using Products.Domain.ValueObjects;
using Products.Shared.DTOs.Category;
using Products.Shared.DTOs.Product;
using Products.Shared.DTOs.ProductVariant;
using Shared.BuildingBlocks.Result;
using Shared.Helpers.Hotchocolate;

namespace Products.API.Types.Mutations;

[MutationType]
[Authorize]
public class ProductsMutations
{
    [Error<ResultError>]
    public async Task<FieldResult<Product>> CreateProduct(ISender sender, CreateProductDto product) =>
        ResultHandler.HandleResponse(await sender.Send(new CreateProductCommand(product)));

    [Error<ResultError>]
    public async Task<FieldResult<Product>> AddProductVariant(ISender sender, AddProductVariantDto productVariant) =>
        ResultHandler.HandleResponse(await sender.Send(new AddProductVariantCommand(productVariant)));

    [Error<ResultError>]
    public async Task<FieldResult<ProductVariantAttribute>> AddProductVariantAttribute(ISender sender, AddProductVariantAttributeDto productVariantAttribute) =>
        ResultHandler.HandleResponse(await sender.Send(new AddProductVariantAttributeCommand(productVariantAttribute)));

    [Error<ResultError>]
    public async Task<FieldResult<Product>> EditProduct(ISender sender, EditProductDto product) => 
        ResultHandler.HandleResponse(await sender.Send(new EditProductCommand(product)));

    [Error<ResultError>]
    public async Task<FieldResult<bool>> DeleteProduct(ISender sender, Guid productId) =>
        ResultHandler.HandleResponse(await sender.Send(new DeleteProductCommand(productId)));
}