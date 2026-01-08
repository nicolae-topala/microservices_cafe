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
        builder.HasOne(x => x.ProductVariant)
            .WithMany(x => x.VariantAttributes)
            .HasForeignKey(x => x.ProductVariantId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired();

        builder.HasOne(x => x.AttributeDefinition)
            .WithMany()
            .HasForeignKey(x => x.AttributeDefinitionId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired();

        builder.HasOne(x => x.UnitsOfMeasure)
            .WithMany()
            .HasForeignKey(x => x.UnitsOfMeasureId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false); 
    }
}