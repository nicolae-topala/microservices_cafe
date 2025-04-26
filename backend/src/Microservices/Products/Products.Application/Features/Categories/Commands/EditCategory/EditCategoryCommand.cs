using Products.Domain.Entities;
using Products.Shared.DTOs.Category;
using Shared.Abstractions.Messaging;
using Shared.Abstractions.Messaging.ResultType;

namespace Products.Application.Features.Categories.Commands.EditCategory;

public record EditCategoryCommand(EditCategoryDto Category) : IResultCommand<Category>
{
}
