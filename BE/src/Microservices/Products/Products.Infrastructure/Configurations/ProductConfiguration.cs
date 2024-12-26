﻿using Products.Domain.Entities;
using Products.Infrastructure.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Products.Infrastructure.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable(TableNames.Products);

        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.Name)
            .HasMaxLength(256)
            .IsRequired();

        builder
            .Property(x => x.Description)
            .HasMaxLength(2000)
            .IsRequired();

        builder.ComplexProperty(x => x.Price, y => 
            { 
                y.IsRequired(); 
            });

        builder
            .Property(x => x.Type)
            .IsRequired();

        builder
            .Property(x => x.IsVisible)
            .IsRequired();

        builder
            .Property(x => x.IsInStock)
            .IsRequired();

        builder
            .HasMany(x => x.Categories)
            .WithMany(c => c.Products);

        builder
            .HasIndex(x => x.Name)
            .IsUnique();
    }
}