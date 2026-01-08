using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Products.Domain.Entities;
using Products.Infrastructure.Constants;

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

        // Relationships
        builder
            .HasMany(x => x.Images)
            .WithOne(x => x.ProductVariant)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(x => x.VariantAttributes)
            .WithOne(x => x.ProductVariant)
            .HasForeignKey(x => x.ProductVariantId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}