using Microsoft.EntityFrameworkCore;
using Products.Application.Abstractions;
using Shared.Abstractions.Messaging;
using Shared.BuildingBlocks.Result;

namespace Products.Application.Features.Categories.Commands.DeleteCategory;

public class DeleteCategoryCommandHandler(IProductsDbContext dbContext)
    : ICommandHandler<DeleteCategoryCommand, bool>
{
    public async Task<Result<bool>> Handle(
        DeleteCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var category = await dbContext.Categories
            .FirstOrDefaultAsync(x => x.Id == request.CategoryId, cancellationToken)
            .ConfigureAwait(false);

        if (category is null)
        {
            return Result.Failure<bool>(new ResultError("", ""));
        }

        dbContext.Categories.Remove(category);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success(true);
    }
}

