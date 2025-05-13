using Price.Shared.DTOs.ProductPrice;
using Shared.Abstractions.Messaging.ResultType;

namespace Price.Application.Features.ProductPrice.Commands.UpdateProductPrice;

public record UpdateProductPriceCommand(UpdateProductPriceDto ProductPrice) : IResultCommand;
