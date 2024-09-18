using Products.Application.Abstractions;
using Products.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Products.Infrastructure;

public class ProductsDbContext : DbContext, IProductsDbContext
{
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Category> Categories => Set<Category>();

    public ProductsDbContext(DbContextOptions<ProductsDbContext> options) : base(options) 
    { 
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfigurationsFromAssembly(AssemblyReference.Assembly);

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => await base.SaveChangesAsync(cancellationToken);
}
