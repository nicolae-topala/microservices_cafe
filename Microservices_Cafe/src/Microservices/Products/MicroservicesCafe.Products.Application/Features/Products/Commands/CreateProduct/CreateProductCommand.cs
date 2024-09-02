using MediatR;
using MicroservicesCafe.Products.Shared.DTOs;
using MicroservicesCafe.Shared.Abstractions.Messaging;
using MicroservicesCafe.Shared.BuildingBlocks.Result;

namespace MicroservicesCafe.Products.Application.Features.Products.Commands.CreateProduct;

public record CreateProductCommand(CreateProductDto Product) : ICommand<ProductDto>
{
}