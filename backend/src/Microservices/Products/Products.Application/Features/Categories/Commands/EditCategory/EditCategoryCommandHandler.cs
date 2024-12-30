using Microsoft.EntityFrameworkCore;
using Products.Application.Abstractions;
using Products.Domain.Entities;
using Products.Shared.Errors;
using Shared.Abstractions.Messaging;
using Shared.BuildingBlocks.Result;

namespace Products.Application.Features.Categories.Commands.EditCategory;
public class EditCategoryCommandHandler(IProductsDbContext dbContext)
    : ICommandHandler<EditCategoryCommand, Category>
{
    public async Task<Result<Category>> Handle(EditCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await dbContext.Categories
            .FirstOrDefaultAsync(x => x.Id == request.Category.Id, cancellationToken)
            .ConfigureAwait(false);

        if (category is null)
        {
            return Result.Failure<Category>(CategoryErrors.CategoryNotFound);
        }

        var categoryResult = category.Edit(
            request.Category.Name
            );

        if (categoryResult.IsFailure)
        {
            return Result.Failure<Category>(categoryResult.Error);
        }

        await dbContext
            .SaveChangesAsync(cancellationToken)
            .ConfigureAwait(false);

        return Result.Success(categoryResult.Value);
    }
}
