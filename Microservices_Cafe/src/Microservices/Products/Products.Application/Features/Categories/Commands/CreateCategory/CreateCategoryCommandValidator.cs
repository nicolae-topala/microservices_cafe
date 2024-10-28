using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Products.Application.Abstractions;

namespace Products.Application.Features.Categories.Commands.CreateCategory;
public sealed class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
	public CreateCategoryCommandValidator(IProductsDbContext dbContext)
	{
        RuleFor(c => c.Category.Name).NotEmpty().WithMessage("Category name can't be empty!");

        RuleFor(c => c.Category.Name).NotEmpty().MustAsync(async (name, cancellationToken) =>
		{
			return !await dbContext.Categories.AnyAsync(x => x.Name == name, cancellationToken);
		}).WithMessage("Category name must be unique!");
	}
}
