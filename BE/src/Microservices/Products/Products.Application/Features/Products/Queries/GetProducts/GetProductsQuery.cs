using Products.Domain.Entities;
using Shared.Abstractions.Messaging;

namespace Products.Application.Features.Products.Queries.GetProducts;

public record GetProductsQuery() : IQuery<IQueryable<Product>>;
