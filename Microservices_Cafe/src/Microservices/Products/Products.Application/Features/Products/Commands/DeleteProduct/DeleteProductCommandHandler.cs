﻿using Microsoft.EntityFrameworkCore;
using Products.Application.Abstractions;
using Shared.Abstractions.Messaging;
using Shared.BuildingBlocks.Result;

namespace Products.Application.Features.Products.Commands.DeleteProduct;

public class DeleteProductCommandHandler(IProductsDbContext dbContext)
    : ICommandHandler<DeleteProductCommand>
{
    public async Task<Result> Handle(
        DeleteProductCommand request,
        CancellationToken cancellationToken)
    {
        var product = await dbContext.Products
            .FirstOrDefaultAsync(x => x.Id == request.ProductId, cancellationToken)
            .ConfigureAwait(false);

        if (product is null)
        {
            return Result.Failure(new Error("", ""));
        }

        dbContext.Products.Remove(product);
        await dbContext.SaveChangesAsync(cancellationToken)
            .ConfigureAwait(false);

        return Result.Success();
    }
}
