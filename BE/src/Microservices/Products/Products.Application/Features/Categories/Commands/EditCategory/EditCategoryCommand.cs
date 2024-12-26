using Products.Domain.Entities;
using Products.Shared.DTOs;
using Shared.Abstractions.Messaging;

namespace Products.Application.Features.Categories.Commands.EditCategory;

public record EditCategoryCommand(EditCategoryDto Category) : ICommand<Category>
{
}
