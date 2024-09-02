using MicroservicesCafe.Shared.Enums;
using MicroservicesCafe.Shared.ValueObjects;

namespace MicroservicesCafe.Products.Shared.DTOs;

public record CreateProductDto(
    string Name,
    string Description,
    Price Price,
    ProductTypeEnum Type,
    List<string> Ingredients,
    Guid CategoryId);
