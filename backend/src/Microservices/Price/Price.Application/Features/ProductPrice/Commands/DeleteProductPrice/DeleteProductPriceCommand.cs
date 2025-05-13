using Shared.Abstractions.Messaging.ResultType;

namespace Price.Application.Features.ProductPrice.Commands.DeleteProductPrice;

public record DeleteProductPriceCommand(Guid ProductPriceId) : IResultCommand;