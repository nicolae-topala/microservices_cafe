using Shared.Abstractions.Messaging;

namespace Products.Application.Features.Products.Commands.DeleteProduct;

public record DeleteProductCommand(Guid ProductId) : ICommand
{
}