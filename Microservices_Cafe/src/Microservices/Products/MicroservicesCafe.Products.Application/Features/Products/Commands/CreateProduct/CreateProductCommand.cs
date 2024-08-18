using MicroservicesCafe.Shared.Abstractions.Messaging;
using MicroservicesCafe.Shared.Enums;
using MicroservicesCafe.Shared.Primitives;
using MicroservicesCafe.Shared.ValueObjects;

namespace MicroservicesCafe.Products.Application.Features.Products.Commands.CreateProduct;

public sealed record CreateProductCommand(
    string Name,
    string Description,
    Price Price,
    ProductTypeEnum Type,
    List<string> Ingredients,
    Guid CategoryId) : ICommand<EntityCreatedResponse>;