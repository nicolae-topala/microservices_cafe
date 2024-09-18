using Shared.Enums;

namespace Products.Shared.DTOs;

public record PriceDto(
    decimal Ammount,
    CurrencyEnum Currency);
