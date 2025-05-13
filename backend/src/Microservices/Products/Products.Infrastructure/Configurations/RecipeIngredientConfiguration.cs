using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Products.Domain.Entities;
using Products.Infrastructure.Constants;

namespace Products.Infrastructure.Configurations;

public class RecipeIngredientConfiguration : IEntityTypeConfiguration<RecipeIngredient>
{
    public void Configure(EntityTypeBuilder<RecipeIngredient> builder)
    {
        builder.ToTable(TableNames.RecipeIngredients);

        builder.HasKey(ri => ri.Id);

        builder.Property(ri => ri.Quantity)
            .IsRequired()
            .HasColumnType("decimal(18,4)");

        // Relationships
        builder
            .HasOne(x => x.UnitsOfMeasure)
            .WithOne()
            .OnDelete(DeleteBehavior.Restrict);

        // Relationship with Recipe
        builder.HasOne(x => x.Recipe)
            .WithMany(r => r.Ingredients)
            .HasForeignKey(x => x.RecipeId)
            .OnDelete(DeleteBehavior.Restrict);

        // Relationship with Product
        //builder.HasOne(x => x.ProductVariant)
        //    .WithMany(r => r.Ingredients)
        //    .OnDelete(DeleteBehavior.Restrict);
    }
}