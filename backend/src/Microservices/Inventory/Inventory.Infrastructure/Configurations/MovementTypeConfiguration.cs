using Inventory.Domain.Entities;
using Inventory.Infrastructure.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Infrastructure.Configurations;
public class MovementTypeConfiguration : IEntityTypeConfiguration<MovementType>
{
    public void Configure(EntityTypeBuilder<MovementType> builder)
    {
        builder.ToTable(TableNames.MovementTypes);

        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.Name)
            .HasMaxLength(256)
            .IsRequired();

        builder
            .Property(x => x.Description)
            .HasMaxLength(2000);
    }
}