using Microsoft.EntityFrameworkCore;
using Products.Application.Abstractions;
using Shared.Abstractions.Messaging.ResultType;
using Shared.BuildingBlocks.Result;

namespace Products.Application.Features.ProductVariants.Commands.SetVisibilityProducts;

public class SetVisibilityProductsHandler(IProductsDbContext dbContext) : IResultCommandHandler<SetVisibilityProductsCommand>
{
    public async Task<Result> Handle(SetVisibilityProductsCommand request, CancellationToken cancellationToken)
    {
        var products = await dbContext.ProductVariants
            .Include(p => p.Product)
            .Where(p => request.Request.ProductIds.Contains(p.Id))
            .ToListAsync(cancellationToken);

        foreach (var product in products)
        {
            product.SetVisibility(request.Request.SetIsVisible);
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
