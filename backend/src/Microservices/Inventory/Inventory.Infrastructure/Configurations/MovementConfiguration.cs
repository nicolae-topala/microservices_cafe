using Inventory.Domain.Entities;
using Inventory.Infrastructure.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Infrastructure.Configurations;

public class MovementConfiguration : IEntityTypeConfiguration<Movement>
{
    public void Configure(EntityTypeBuilder<Movement> builder)
    {
        builder.ToTable(TableNames.Movements);

        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.Quantity)
            .IsRequired();

        builder
            .Property(x => x.MovementDate)
            .IsRequired();

        // Relationships
        builder
            .HasOne(x => x.Item)
            .WithMany()
            .HasForeignKey(x => x.ItemId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.MovementType)
            .WithMany()
            .HasForeignKey(x => x.MovementTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.Location)
            .WithMany()
            .HasForeignKey(x => x.LocationId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}