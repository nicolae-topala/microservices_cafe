using HotChocolate.Authorization;
using MediatR;
using Products.Application.Features.Products.Commands.CreateProduct;
using Products.Application.Features.Products.Commands.DeleteProduct;
using Products.Application.Features.Products.Commands.EditProduct;
using Products.Domain.Entities;
using Products.Shared.DTOs;

namespace Products.API.Types.Mutations;

[MutationType]
[Authorize]
public class ProductsMutations
{
    public async Task<Product> CreateProduct([Service] ISender sender, CreateProductDto product)
    {
        var result = await sender.Send(new CreateProductCommand(product));
        return result.Value;
    }

    public async Task<Product> EditProduct([Service] ISender sender, EditProductDto product)
    {
        var result = await sender.Send(new EditProductCommand(product));
        return result.Value;
    }

    public async Task DeleteProduct([Service] ISender sender, Guid productId) =>
        await sender.Send(new DeleteProductCommand(productId));
}

