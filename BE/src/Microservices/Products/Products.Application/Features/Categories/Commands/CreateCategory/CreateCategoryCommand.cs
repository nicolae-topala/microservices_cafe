using Products.Domain.Entities;
using Products.Shared.DTOs;
using Shared.Abstractions.Messaging;

namespace Products.Application.Features.Categories.Commands.CreateCategory;

public record CreateCategoryCommand(CreateCategoryDto Category) : ICommand<Category>
{
}