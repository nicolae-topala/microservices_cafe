using Shared.Enums;

namespace Products.Shared.DTOs.Category;
public record EditProductDto(
    Guid Id,
    string? Name,
    string? Description,
    decimal? Price,
    Currency? Currency,
    ProductType? Type,
    List<string>? Ingredients,
    Guid? CategoryId,
    bool? IsVisible,
    bool? IsInStock);
