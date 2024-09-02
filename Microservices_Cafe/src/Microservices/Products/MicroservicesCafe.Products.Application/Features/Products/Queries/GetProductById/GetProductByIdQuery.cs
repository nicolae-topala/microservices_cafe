using MicroservicesCafe.Products.Shared.DTOs;
using MicroservicesCafe.Shared.Abstractions.Messaging;

namespace MicroservicesCafe.Products.Application.Features.Products.Queries.GetProductById;

public record GetProductByIdQuery(Guid Id) : IQuery<ProductDto>;
