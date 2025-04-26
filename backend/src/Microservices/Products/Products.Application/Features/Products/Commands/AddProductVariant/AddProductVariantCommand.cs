using Products.Domain.Entities;
using Products.Shared.DTOs.Product;
using Shared.Abstractions.Messaging.ResultType;

namespace Products.Application.Features.Products.Commands.AddProductVariant;

public record AddProductVariantCommand(AddProductVariantDto ProductVariant) : IResultCommand<Product>;
