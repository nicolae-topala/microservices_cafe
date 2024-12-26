using Shared.Enums;

namespace Products.Shared.DTOs;

public record CreateProductDto(
    string Name,
    string Description,
    decimal Price,
    CurrencyEnum Currency,
    ProductTypeEnum Type,
    List<string> Ingredients,
    List<Guid> CategoryIds);
