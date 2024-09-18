using Shared.Enums;

namespace Products.Shared.DTOs;

public record ProductDto(
    Guid Id,
    string Name,
    string Description,
    PriceDto Price,
    ProductTypeEnum Type,
    IReadOnlyCollection<string> Ingredients,
    Guid CategoryId);
