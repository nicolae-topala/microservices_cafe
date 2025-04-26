using Shared.Abstractions.Messaging.ResultType;

namespace Products.Application.Features.Products.Commands.DeleteProduct;

public record DeleteProductCommand(Guid ProductId) : IResultCommand
{
}