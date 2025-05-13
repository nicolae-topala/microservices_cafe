using Microsoft.EntityFrameworkCore;
using Products.Domain.Entities;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Products.Application.Abstractions;

public interface IProductsDbContext
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
    DatabaseFacade Database { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
