using Shared.Enums;

namespace Products.Shared.DTOs.Product;

public record CreateProductDto(
    string Name,
    string Description,
    ProductType Type,
    List<string> Ingredients,
    List<Guid> CategoryIds);
