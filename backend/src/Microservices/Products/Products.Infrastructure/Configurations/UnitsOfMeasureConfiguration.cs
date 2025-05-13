using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Products.Domain.Entities;
using Products.Infrastructure.Constants;

namespace Products.Infrastructure.Configurations;

public class UnitsOfMeasureConfiguration : IEntityTypeConfiguration<UnitsOfMeasure>
{
    public void Configure(EntityTypeBuilder<UnitsOfMeasure> builder)
    {
        builder.ToTable(TableNames.UnitsOfMeasures);

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(r => r.Description)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(r => r.Abbreviation)
            .IsRequired()
            .HasMaxLength(10);
    }
}