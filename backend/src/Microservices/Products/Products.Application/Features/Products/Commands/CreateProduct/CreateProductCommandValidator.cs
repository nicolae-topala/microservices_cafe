using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Products.Application.Abstractions;

namespace Products.Application.Features.Products.Commands.CreateProduct;

public sealed class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator(IProductsDbContext dbContext)
    {
        RuleFor(x => x.Product.Name)
            .MustAsync(async (name, cancellationToken) =>
            {
                return !await dbContext.Products
                    .AnyAsync(p => p.Name == name, cancellationToken);
            })
            .WithMessage("A product with this name already exists.");
    }
}