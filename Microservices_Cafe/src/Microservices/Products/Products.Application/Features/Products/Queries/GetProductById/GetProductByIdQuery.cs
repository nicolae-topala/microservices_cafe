using Products.Shared.DTOs;
using Shared.Abstractions.Messaging;

namespace Products.Application.Features.Products.Queries.GetProductById;

public record GetProductByIdQuery(Guid Id) : IQuery<ProductDto>;
