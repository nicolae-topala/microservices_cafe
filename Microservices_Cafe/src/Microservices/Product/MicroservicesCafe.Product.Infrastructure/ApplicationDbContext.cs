using MicroservicesCafe.Product.Application.Abstractions;
using MicroservicesCafe.Product.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Products = MicroservicesCafe.Product.Domain.Entities.Product;


namespace MicroservicesCafe.Product.Persistance;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public DbSet<Products> Products => Set<Products>();
    public DbSet<Category> Categories => Set<Category>();

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfigurationsFromAssembly(AssemblyReference.Assembly);
}
