using Products.Domain.Entities;
using Shared.Abstractions.Messaging;

namespace Products.Application.Features.Categories.Queries.GetCategories;

public record GetCategoriesQuery : IQuery<IQueryable<Category>>;
