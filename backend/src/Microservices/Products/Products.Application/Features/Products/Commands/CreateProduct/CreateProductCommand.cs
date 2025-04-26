using Products.Domain.Entities;
using Products.Shared.DTOs.Product;
using Shared.Abstractions.Messaging.ResultType;

namespace Products.Application.Features.Products.Commands.CreateProduct;

public record CreateProductCommand(CreateProductDto Product) : IResultCommand<Product>
{
}