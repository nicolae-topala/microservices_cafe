using Shared.Abstractions.Messaging;

namespace Products.Application.Features.Categories.Commands.DeleteCategory;

public record DeleteCategoryCommand(Guid CategoryId) : ICommand<bool>
{
}