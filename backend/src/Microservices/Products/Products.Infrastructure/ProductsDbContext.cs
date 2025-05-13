using Products.Application.Abstractions;
using Products.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Products.Infrastructure;

public class ProductsDbContext(DbContextOptions<ProductsDbContext> options) 
    : DbContext(options), IProductsDbContext
{
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();

    public DbSet<ProductVariant> ProductVariants => Set<ProductVariant>();

    public DbSet<ProductImage> ProductImages => Set<ProductImage>();

    public DbSet<ProductVariantAttribute> ProductVariantAttributes => Set<ProductVariantAttribute>();

    public DbSet<Recipe> Recipes => Set<Recipe>();

    public DbSet<RecipeIngredient> RecipeIngredients => Set<RecipeIngredient>();

    public DbSet<UnitsOfMeasure> UnitsOfMeasures => Set<UnitsOfMeasure>();

    public DbSet<VariantAttributeDefinition> VariantAttributeDefinitions => Set<VariantAttributeDefinition>();

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfigurationsFromAssembly(AssemblyReference.Assembly);

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => 
        await base.SaveChangesAsync(cancellationToken);
}
