using Products.Shared.DTOs.Product;
using Shared.Abstractions.Messaging.ResultType;

namespace Products.Application.Features.ProductVariants.Commands.SetVisibilityProducts;

public record SetVisibilityProductsCommand(SetVisibilityDto Request) : IResultCommand;