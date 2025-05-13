using Products.Shared.DTOs.ProductVariant;
using Shared.Abstractions.Messaging.ResultType;

namespace Products.Application.Features.ProductVariants.Commands;

public record AddProductVariantAttributeCommand(AddProductVariantAttributeDto ProductVariantAttribute) : IResultCommand;