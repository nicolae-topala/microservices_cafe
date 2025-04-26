using Products.Domain.Entities;
using Products.Shared.DTOs.Category;
using Shared.Abstractions.Messaging.ResultType;

namespace Products.Application.Features.Products.Commands.EditProduct;

public record EditProductCommand(EditProductDto Product) : IResultCommand<Product>
{
}
