using MediatR;
using Products.Shared.DTOs;
using Shared.Abstractions.Messaging;
using Shared.BuildingBlocks.Result;

namespace Products.Application.Features.Products.Commands.CreateProduct;

public record CreateProductCommand(CreateProductDto Product) : ICommand<ProductDto>
{
}