using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Products.Application.Abstractions;

namespace Products.Application.Features.Products.Commands.SetVisibilityProducts;

public sealed class SetVisibilityProductsValidator : AbstractValidator<SetVisibilityProductsCommand>
{
    public SetVisibilityProductsValidator(IProductsDbContext dbContext)
    {
        RuleFor(x => x.Request.ProductIds)
            .MustAsync(async (ids, cancellationToken) =>
            {
                return await dbContext.Products.Where(p => ids.Contains(p.Id))
                    .CountAsync(cancellationToken) != ids.Count;
            })
            .WithMessage("One or more products do not exist.");
    }
}