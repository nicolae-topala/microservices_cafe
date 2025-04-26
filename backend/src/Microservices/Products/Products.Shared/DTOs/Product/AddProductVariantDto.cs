using Shared.Enums;

namespace Products.Shared.DTOs.Product;

public record AddProductVariantDto(
    Guid ProductId,
    decimal Price,
    Currency Currency);