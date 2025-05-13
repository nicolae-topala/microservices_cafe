using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Products.Domain.Entities;
using Products.Infrastructure.Constants;

namespace Products.Infrastructure.Configurations;

public class RecipeConfiguration : IEntityTypeConfiguration<Recipe>
{
    public void Configure(EntityTypeBuilder<Recipe> builder)
    {
        builder.ToTable(TableNames.Recipes);

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(r => r.Description)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(r => r.Instructions)
            .IsRequired()
            .HasMaxLength(4000);

        builder.Property(r => r.PreparationTimeInMinutes)
            .IsRequired();

        builder.Property(r => r.IsPublished)
            .IsRequired();
    }
}