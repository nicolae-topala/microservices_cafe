using Microsoft.EntityFrameworkCore;
using Products.Application.Abstractions;
using Products.Domain.Entities;
using Products.Shared.Errors;
using Shared.Abstractions.Messaging.ResultType;
using Shared.BuildingBlocks.Result;

namespace Products.Application.Features.ProductVariants.Commands.AddProductVariantAttribute;

public class AddProductVariantAttributeHandler(IProductsDbContext dbContext)
    : IResultCommandHandler<AddProductVariantAttributeCommand>
{
    public async Task<Result> Handle(AddProductVariantAttributeCommand request, CancellationToken cancellationToken)
    {
        var productVariant = await dbContext.ProductVariants
            .Include(x => x.VariantAttributes)
            .Include(x => x.Product)
            .FirstOrDefaultAsync(x =>
                x.Id == request.ProductVariantAttribute.ProductVariantId,
                cancellationToken);

        if (productVariant is null)
        {
            return Result.Failure(ProductErrors.NotFound);
        }

        UnitsOfMeasure? unitsOfMeasure = null;
        if (request.ProductVariantAttribute.UnitsOfMeasureId != null)
        {
            unitsOfMeasure = await dbContext.UnitsOfMeasures
                .AsNoTracking()
                .FirstOrDefaultAsync(x =>
                    x.Id == request.ProductVariantAttribute.UnitsOfMeasureId,
                    cancellationToken);
        }

        var attributeDefinition = await dbContext.VariantAttributeDefinitions
            .AsNoTracking()
            .FirstOrDefaultAsync(x =>
                x.Id == request.ProductVariantAttribute.AttributeDefinitionId,
                cancellationToken);

        if (attributeDefinition is null)
        {
            return Result.Failure(ProductErrors.NotFound);
        }

        var result = productVariant.AddOrUpdateVariantAttribute(attributeDefinition, request.ProductVariantAttribute.Value, unitsOfMeasure);

        if (result.IsFailure)
        {
            return Result.Failure(result.Error);
        }

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}