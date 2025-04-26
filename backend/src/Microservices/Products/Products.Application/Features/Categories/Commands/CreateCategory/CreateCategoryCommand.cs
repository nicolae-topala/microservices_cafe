using Products.Domain.Entities;
using Products.Shared.DTOs.Product;
using Shared.Abstractions.Messaging;
using Shared.Abstractions.Messaging.ResultType;

namespace Products.Application.Features.Categories.Commands.CreateCategory;

public record CreateCategoryCommand(CreateCategoryDto Category) : IResultCommand<Category>
{
}