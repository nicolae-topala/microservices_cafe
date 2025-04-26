using Shared.Abstractions.Messaging;
using Shared.Abstractions.Messaging.ResultType;

namespace Products.Application.Features.Categories.Commands.DeleteCategory;

public record DeleteCategoryCommand(Guid CategoryId) : IResultCommand<bool>
{
}