using Shared.Enums;

namespace Products.Shared.DTOs;
public record EditProductDto(
    Guid Id,
    string? Name,
    string? Description,
    decimal? Price,
    CurrencyEnum? Currency,
    ProductTypeEnum? Type,
    List<string>? Ingredients,
    Guid? CategoryId,
    bool? IsVisible,
    bool? IsInStock);
