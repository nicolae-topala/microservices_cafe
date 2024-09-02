using Mapster;
using MicroservicesCafe.Products.Application.Abstractions;
using MicroservicesCafe.Products.Domain.Entities;
using MicroservicesCafe.Products.Shared.DTOs;
using MicroservicesCafe.Shared.Abstractions.Messaging;
using MicroservicesCafe.Shared.BuildingBlocks.Result;

namespace MicroservicesCafe.Products.Application.Features.Products.Commands.CreateProduct;

public class CreateProductCommandHandler(IProductsDbContext dbContext) 
    : ICommandHandler<CreateProductCommand, ProductDto>
{
    public async Task<Result<ProductDto>> Handle(
        CreateProductCommand request, 
        CancellationToken cancellationToken)
    {
        var productResult = Product.Create(
            request.Product.Name, 
            request.Product.Description, 
            request.Product.Price, 
            request.Product.Type, 
            request.Product.CategoryId);

        if (productResult.IsFailure) 
        {
            return Result.Failure<ProductDto>(productResult.Error);
        }

        await dbContext.Products
            .AddAsync(productResult.Value, cancellationToken)
            .ConfigureAwait(false);

        await dbContext.SaveChangesAsync(cancellationToken);

        var productDto = productResult.Value.Adapt<ProductDto>();
        return Result.Success(productDto);
    }
}

