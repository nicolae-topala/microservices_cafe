using Microsoft.EntityFrameworkCore;
using Products.Application.Abstractions;
using Products.Domain.Entities;
using Products.Domain.ValueObjects;
using Products.Shared.Errors;
using Shared.Abstractions.Messaging.ResultType;
using Shared.BuildingBlocks.Result;

namespace Products.Application.Features.Products.Commands.AddProductVariant;

public class AddProductVariantHandler(IProductsDbContext dbContext)
    : IResultCommandHandler<AddProductVariantCommand, Product>
{
    public async Task<Result<Product>> Handle(AddProductVariantCommand request, CancellationToken cancellationToken)
    {
        var product = await dbContext.Products
            .Include(x => x.Variants)
            .FirstOrDefaultAsync(x =>
                x.Id == request.ProductVariant.ProductId,
                cancellationToken);

        if (product is null)
        {
            return Result.Failure<Product>(ProductErrors.NotFound);
        }

        var result = product.AddVariant(
            request.ProductVariant.Price, 
            request.ProductVariant.Currency, 
            new List<ProductVariantAttribute>());

        if (result.IsFailure)
        {
            return Result.Failure<Product>(result.Error);
        }

        await dbContext.ProductVariants
            .AddAsync(result.Value, cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(product);
    }
}