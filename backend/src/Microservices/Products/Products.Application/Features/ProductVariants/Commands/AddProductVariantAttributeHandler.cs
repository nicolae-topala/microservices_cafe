using Microsoft.EntityFrameworkCore;
using Products.Application.Abstractions;
using Products.Domain.ValueObjects;
using Products.Shared.Errors;
using Shared.Abstractions.Messaging.ResultType;
using Shared.BuildingBlocks.Result;

namespace Products.Application.Features.ProductVariants.Commands;

public class AddProductVariantAttributeHandler(IProductsDbContext dbContext)
    : IResultCommandHandler<AddProductVariantAttributeCommand, ProductVariantAttribute>
{
    public async Task<Result<ProductVariantAttribute>> Handle(AddProductVariantAttributeCommand request, CancellationToken cancellationToken)
    {
        var productVariant = await dbContext.ProductVariants
            .Include(x => x.VariantAttributes)
            .FirstOrDefaultAsync(x =>
                x.Id == request.ProductVariantAttribute.ProductVariantId,
                cancellationToken);

        if (productVariant is null)
        {
            return Result.Failure<ProductVariantAttribute>(ProductErrors.NotFound);
        }

        var attributeResult = ProductVariantAttribute.Create(
            request.ProductVariantAttribute.Name,
            request.ProductVariantAttribute.Type,
            request.ProductVariantAttribute.Value);

        if (attributeResult.IsFailure)
        {
            return Result.Failure<ProductVariantAttribute>(attributeResult.Error);
        }

        productVariant.AddOrUpdateVariantAttribute(attributeResult.Value);

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(attributeResult.Value);
    }
}