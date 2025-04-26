using Microsoft.EntityFrameworkCore;
using Products.Application.Abstractions;
using Products.Domain.Entities;
using Shared.Abstractions.Messaging.ResultType;
using Shared.BuildingBlocks.Result;

namespace Products.Application.Features.Products.Commands.EditProduct;
public class EditProductCommandHandler(IProductsDbContext dbContext)
    : IResultCommandHandler<EditProductCommand, Product>
{
    public async Task<Result<Product>> Handle(EditProductCommand request, CancellationToken cancellationToken)
    {
        var product = await dbContext.Products
            .FirstOrDefaultAsync(x => x.Id == request.Product.Id, cancellationToken)
            .ConfigureAwait(false);

        if (product is null)
        {
            return Result.Failure<Product>(new ResultError("", ""));
        }

        var productResult = product.Edit(
            request.Product.Name,
            request.Product.Description,
            request.Product.Type,
            new List<Category>(),
            request.Product.IsVisible,
            request.Product.IsInStock);

        if (productResult.IsFailure)
        {
            return Result.Failure<Product>(productResult.Error);
        }

        await dbContext
            .SaveChangesAsync(cancellationToken)
            .ConfigureAwait(false);

        return Result.Success(productResult.Value);
    }
}
