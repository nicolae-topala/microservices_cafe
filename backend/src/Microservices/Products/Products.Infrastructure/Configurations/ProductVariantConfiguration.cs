using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Products.Domain.Entities;
using Products.Infrastructure.Constants;
using Products.Domain.ValueObjects;

namespace Products.Infrastructure.Configurations;

public class ProductVariantConfiguration : IEntityTypeConfiguration<ProductVariant>
{
    public void Configure(EntityTypeBuilder<ProductVariant> builder)
    {
        builder.ToTable(TableNames.ProductVariants);

        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.ProductId)
            .IsRequired();

        builder
            .Property(x => x.IsInStock)
            .IsRequired();

        builder
            .Property(x => x.IsVisible)
            .IsRequired();

        // Value Objects
        builder.ComplexProperty(x => x.Price, complexBuilder =>
        {
            complexBuilder.IsRequired();
        });

        builder.OwnsMany(x => x.VariantAttributes, navigationBuilder =>
        {
            navigationBuilder
                .Property(va => va.Name)
                .IsRequired();

            navigationBuilder
                .Property(va => va.Key)
                .IsRequired();

            navigationBuilder
                .HasKey(
                    "ProductVariantId", 
                    nameof(ProductVariantAttribute.Key), 
                    nameof(ProductVariantAttribute.Name));
        });

        // Relationships
        builder
            .HasMany(x => x.Images)
            .WithOne()
            .HasForeignKey(x => x.VariantId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}