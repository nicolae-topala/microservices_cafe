using Price.Shared.DTOs.ProductPrice;
using Shared.Abstractions.Messaging.ResultType;
using ProductPriceDomain = Price.Domain.Entities.ProductPrice;

namespace Price.Application.Features.ProductPrice.Commands.CreateProductPrice;
public record CreateProductPriceCommand(CreateProductPriceDto ProductPrice) : IResultCommand<ProductPriceDomain>;
