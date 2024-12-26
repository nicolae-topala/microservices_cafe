using Products.Application.Abstractions;
using Products.Domain.Entities;
using Shared.Abstractions.Messaging;
using Shared.BuildingBlocks.Result;

namespace Products.Application.Features.Categories.Commands.CreateCategory;

public class DeleteProductCommandHandler(IProductsDbContext dbContext)
    : ICommandHandler<CreateCategoryCommand, Category>
{
    public async Task<Result<Category>> Handle(
        CreateCategoryCommand request,
        CancellationToken cancellationToken)
    {
        var categoryResult = Category.Create(request.Category.Name);

        if (categoryResult.IsFailure)
        {
            return Result.Failure<Category>(categoryResult.Error);
        }

        await dbContext.Categories
            .AddAsync(categoryResult.Value, cancellationToken)
            .ConfigureAwait(false);

        await dbContext.SaveChangesAsync(cancellationToken)
            .ConfigureAwait(false);

        return Result.Success(categoryResult.Value);
    }
}
