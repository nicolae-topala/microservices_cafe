using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Price.Domain.Entities;
using Price.Infrastructure.Constants;

namespace Price.Infrastructure.Configurations;

internal class DiscountRuleConfiguration : IEntityTypeConfiguration<DiscountRule>
{
    public void Configure(EntityTypeBuilder<DiscountRule> builder)
    {
        builder.ToTable(TableNames.DiscountRules);

        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.ProductVariantId);

        builder
            .Property(x => x.ProductCategoryId);

        builder
            .Property(x => x.DiscountPercentage)
            .HasPrecision(5, 2)
            .IsRequired();

        builder
            .Property(x => x.Condition)
            .HasMaxLength(1000)
            .IsRequired();

        builder
            .Property(x => x.EffectiveFrom)
            .IsRequired();

        builder
            .Property(x => x.EffectiveTo)
            .IsRequired();

        // Relationships
        builder
            .HasOne(x => x.Channel)
            .WithMany()
            .HasForeignKey(x => x.ChannelId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}