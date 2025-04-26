using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Products.Domain.Entities;
using Products.Infrastructure.Constants;

namespace Products.Infrastructure.Configurations;

public class ProductImageConfiguration : IEntityTypeConfiguration<ProductImage>
{
    public void Configure(EntityTypeBuilder<ProductImage> builder)
    {
        builder.ToTable(TableNames.ProductImages);

        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.VariantId)
            .IsRequired();

        builder
            .Property(x => x.ImageUrl)
            .HasMaxLength(255)
            .IsRequired();

        builder
            .Property(x => x.AltText)
            .HasMaxLength(255)
            .IsRequired(false);

        builder
            .Property(x => x.SortOrder)
            .IsRequired();

        // Indexes
        builder
            .HasIndex(x => x.VariantId);
    }
}