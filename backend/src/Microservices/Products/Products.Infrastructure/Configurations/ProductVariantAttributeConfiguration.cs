using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Products.Domain.Entities;

namespace Products.Infrastructure.Configurations;

public class ProductVariantAttributeConfiguration : IEntityTypeConfiguration<ProductVariantAttribute>
{
    public void Configure(EntityTypeBuilder<ProductVariantAttribute> builder)
    {
        builder.ToTable("ProductVariantAttribute");

        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.Value)
            .IsRequired();

        // Relationships
        builder
            .HasOne(x => x.UnitsOfMeasure)
            .WithOne()
            .HasForeignKey<ProductVariantAttribute>(x => x.UnitsOfMeasureId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.AttributeDefinition)
            .WithOne()
            .HasForeignKey<ProductVariantAttribute>(x => x.AttributeDefinitionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}