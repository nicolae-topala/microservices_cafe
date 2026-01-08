using Products.Shared.DTOs.ProductVariant;
using Shared.Abstractions.Messaging.ResultType;

namespace Products.Application.Features.ProductVariants.Commands.AddProductVariantAttribute;

public record AddProductVariantAttributeCommand(AddProductVariantAttributeDto ProductVariantAttribute) : IResultCommand;