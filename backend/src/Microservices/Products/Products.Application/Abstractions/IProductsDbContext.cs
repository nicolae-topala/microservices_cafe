﻿using Microsoft.EntityFrameworkCore;
using Products.Domain.Entities;
using Shared.Abstractions;

namespace Products.Application.Abstractions;

public interface IProductsDbContext : IDbContext, IHasOutboxMessages
{
    DbSet<Product> Products { get; }
    DbSet<ProductVariant> ProductVariants { get; }
    DbSet<ProductImage> ProductImages { get; }
    DbSet<Category> Categories { get; }
    DbSet<ProductVariantAttribute> ProductVariantAttributes { get; }
    DbSet<Recipe> Recipes { get; }
    DbSet<RecipeIngredient> RecipeIngredients { get; }
    DbSet<UnitsOfMeasure> UnitsOfMeasures { get; }
    DbSet<VariantAttributeDefinition> VariantAttributeDefinitions { get; }
}
