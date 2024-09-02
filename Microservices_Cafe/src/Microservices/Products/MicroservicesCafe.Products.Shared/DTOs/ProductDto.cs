using MicroservicesCafe.Shared.Enums;
using MicroservicesCafe.Shared.ValueObjects;

namespace MicroservicesCafe.Products.Shared.DTOs;

public record ProductDto(
    Guid Id,
    string Name,
    string Description,
    Price Price,
    ProductTypeEnum Type,
    List<string> Ingredients,
    Guid CategoryId);
