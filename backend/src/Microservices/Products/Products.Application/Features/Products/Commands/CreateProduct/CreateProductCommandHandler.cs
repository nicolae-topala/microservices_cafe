using Microsoft.EntityFrameworkCore;
using Products.Application.Abstractions;
using Products.Domain.Entities;
using Shared.Abstractions.Messaging.ResultType;
using Shared.BuildingBlocks.Result;

namespace Products.Application.Features.Products.Commands.CreateProduct;

public class CreateProductCommandHandler(IProductsDbContext dbContext)
    : IResultCommandHandler<CreateProductCommand, Product>
{
    public async Task<Result<Product>> Handle(
        CreateProductCommand request,
        CancellationToken cancellationToken)
    {
        var categories = await dbContext.Categories
            .Where(x => request.Product.CategoryIds.Contains(x.Id))
            .ToListAsync(cancellationToken);

        if (categories.Count != request.Product.CategoryIds.Count)
        {
            return Result.Failure<Product>(new ResultError("Category.NotFound", "One or more categories not found."));
        }

        var productResult = Product.Create(
            request.Product.Name,
            request.Product.Description,
            request.Product.Type,
            categories,
            request.Product.VariantPrice,
            request.Product.VariantCurrency);

        if (productResult.IsFailure)
        {
            return Result.Failure<Product>(productResult.Error);
        }

        dbContext.Products.Add(productResult.Value);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(productResult.Value);
    }
}

