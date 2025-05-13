using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Price.Domain.Entities;
using Price.Infrastructure.Constants;

namespace Price.Infrastructure.Configurations;

internal class ProductPriceConfiguration : IEntityTypeConfiguration<ProductPrice>
{
    public void Configure(EntityTypeBuilder<ProductPrice> builder)
    {
        builder.ToTable(TableNames.ProductPrices);

        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.ProductVariantId)
            .IsRequired();

        builder
            .Property(x => x.EffectiveFrom)
            .IsRequired();

        builder
            .Property(x => x.EffectiveTo)
            .IsRequired();

        // Value Objects
        builder.ComplexProperty(x => x.Price, complexBuilder =>
        {
            complexBuilder.IsRequired();
        });

        // Relationships
        builder
            .HasOne(x => x.Channel)
            .WithMany()
            .HasForeignKey(x => x.ChannelId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}