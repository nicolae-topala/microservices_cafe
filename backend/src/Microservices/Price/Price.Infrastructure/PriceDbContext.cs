using Microsoft.EntityFrameworkCore;
using Price.Application.Abstractions;
using Price.Domain.Entities;

namespace Price.Infrastructure;
public class PriceDbContext(DbContextOptions<PriceDbContext> options)
    : DbContext(options), IPriceDbContext
{
    public DbSet<Channel> Channels => Set<Channel>();
    public DbSet<DiscountRule> DiscountRules => Set<DiscountRule>();
    public DbSet<ProductPrice> ProductPrices => Set<ProductPrice>();

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfigurationsFromAssembly(AssemblyReference.Assembly);

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        await base.SaveChangesAsync(cancellationToken);
}