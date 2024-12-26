using Products.Domain.Entities;
using Products.Infrastructure.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Products.Infrastructure.Configurations;

internal class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable(TableNames.Categories);

        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.Name)
            .HasMaxLength(64)
            .IsRequired();

        builder
            .HasMany(x => x.Products)
            .WithMany(x => x.Categories);

        builder
           .HasMany(c => c.SubCategories)
           .WithOne(c => c.ParentCategory)
           .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasIndex(x => x.Name)
            .IsUnique();
    }
}
