using Products.Domain.Entities;
using Products.Shared.DTOs;
using Shared.Abstractions.Messaging;

namespace Products.Application.Features.Products.Commands.CreateProduct;

public record CreateProductCommand(CreateProductDto Product) : ICommand<Product>
{
}