using Microsoft.EntityFrameworkCore;
using Products.Application.Abstractions;
using Products.Domain.Entities;
using Shared.Abstractions.Messaging;
using Shared.BuildingBlocks.Result;
using Shared.ValueObjects;

namespace Products.Application.Features.Products.Commands.CreateProduct;

public class DeleteProductCommandHandler(IProductsDbContext dbContext)
    : ICommandHandler<CreateProductCommand, Product>
{
    public async Task<Result<Product>> Handle(
        CreateProductCommand request,
        CancellationToken cancellationToken)
    {
        var price = Price.Create(request.Product.Price, request.Product.Currency);

        if (price.IsFailure)
        {
            return Result.Failure<Product>(price.Error);
        }

        var categories = await dbContext.Categories
            .Where(x => request.Product.CategoryIds.Contains(x.Id))
            .ToListAsync(cancellationToken);

        var productResult = Product.Create(
            request.Product.Name,
            request.Product.Description,
            price.Value,
            request.Product.Type,
            categories);

        if (productResult.IsFailure)
        {
            return Result.Failure<Product>(productResult.Error);
        }

        await dbContext.Products
            .AddAsync(productResult.Value, cancellationToken)
            .ConfigureAwait(false);

        await dbContext.SaveChangesAsync(cancellationToken)
            .ConfigureAwait(false);
        return Result.Success(productResult.Value);
    }
}

