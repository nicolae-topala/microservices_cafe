using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Products.Domain.Entities;
using Products.Infrastructure.Constants;

namespace Products.Infrastructure.Configurations;

public class VariantAttributeDefinitionConfiguration : IEntityTypeConfiguration<VariantAttributeDefinition>
{
    public void Configure(EntityTypeBuilder<VariantAttributeDefinition> builder)
    {
        builder.ToTable(TableNames.VariantAttributeDefinitions);

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(r => r.Description)
            .IsRequired()
            .HasMaxLength(2000);
    }
}