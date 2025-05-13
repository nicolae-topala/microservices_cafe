using Inventory.Domain.Entities;
using Inventory.Infrastructure.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Infrastructure.Configurations;
public class LocationConfiguration : IEntityTypeConfiguration<Location>
{
    public void Configure(EntityTypeBuilder<Location> builder)
    {
        builder.ToTable(TableNames.Locations);

        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.Name)
            .HasMaxLength(256)
            .IsRequired();

        // Value Objects==
        builder.ComplexProperty(x => x.Address, complexBuilder =>
        {
            complexBuilder.IsRequired();
        });

        // Relationships
        builder
            .HasOne(x => x.LocationType)
            .WithMany()
            .HasForeignKey(x => x.LocationTypeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}