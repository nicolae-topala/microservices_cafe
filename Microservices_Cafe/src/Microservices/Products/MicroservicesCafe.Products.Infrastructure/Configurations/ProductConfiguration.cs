using MicroservicesCafe.Products.Domain.Entities;
using MicroservicesCafe.Products.Infrastructure.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MicroservicesCafe.Products.Infrastructure.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable(TableNames.Products);

        builder.HasKey(x => x.Id);

        builder
            .Property(x => x.Name)
            .IsRequired();

        builder
            .Property(x => x.Description)
            .IsRequired();

        builder.ComplexProperty(x => x.Price, y => 
            { 
                y.IsRequired(); 
            });

        builder
            .Property(x => x.Type)
            .IsRequired();

        builder
            .HasOne<Category>()
            .WithMany()
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
