using MapsterMapper;
using Products.Application.Abstractions;
using Products.Domain.Entities;
using Products.Shared.DTOs;
using Shared.Abstractions.Messaging;
using Shared.BuildingBlocks.Result;

namespace Products.Application.Features.Products.Commands.CreateProduct;

public class CreateProductCommandHandler(IProductsDbContext dbContext, IMapper mapper) 
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
            .AddAsync(productResult.Value!, cancellationToken)
            .ConfigureAwait(false);

        await dbContext.SaveChangesAsync(cancellationToken);

        var productDto = mapper.Map<ProductDto>(productResult.Value!);
        return Result.Success(productDto);
    }
}

