using Shared.Enums;
using Shared.ValueObjects;

namespace Products.Shared.DTOs;

public record CreateProductDto(
    string Name,
    string Description,
    Price Price,
    ProductTypeEnum Type,
    List<string> Ingredients,
    Guid CategoryId);
