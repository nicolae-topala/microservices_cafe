using Products.Domain.Entities;
using Products.Shared.DTOs;
using Shared.Abstractions.Messaging;

namespace Products.Application.Features.Products.Commands.EditProduct;

public record EditProductCommand(EditProductDto Product) : ICommand<Product>
{
}
