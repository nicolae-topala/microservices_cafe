using Inventory.Domain.Entities;
using Inventory.Infrastructure.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Infrastructure.Configurations;

public class ItemConfiguration : IEntityTypeConfiguration<Item>
{
    public void Configure(EntityTypeBuilder<Item> builder)
    {
        builder.ToTable(TableNames.Items);

        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.Quantity)
            .IsRequired();

        builder
            .Property(x => x.ExpiryDate);

        builder
            .Property(x => x.ProductVariantId)
            .IsRequired();

        // Relationships
        builder
            .HasOne(x => x.Location)
            .WithMany()
            .HasForeignKey(x => x.LocationId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}