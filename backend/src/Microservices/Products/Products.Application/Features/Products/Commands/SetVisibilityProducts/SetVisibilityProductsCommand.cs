using Products.Shared.DTOs.Product;
using Shared.Abstractions.Messaging.ResultType;

namespace Products.Application.Features.Products.Commands.SetVisibilityProducts;

public record SetVisibilityProductsCommand(SetVisibilityDto Request) : IResultCommand;