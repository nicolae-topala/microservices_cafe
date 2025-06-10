using Microsoft.EntityFrameworkCore;
using Price.Application.Abstractions;
using Price.Domain.Entities;
using Shared.BuildingBlocks.Outbox;

namespace Price.Infrastructure;
public class PriceDbContext(DbContextOptions<PriceDbContext> options)
    : DbContext(options), IPriceDbContext
{
    public DbSet<Channel> Channels => Set<Channel>();
    public DbSet<DiscountRule> DiscountRules => Set<DiscountRule>();
    public DbSet<ProductPrice> ProductPrices => Set<ProductPrice>();
    public DbSet<OutboxMessage> OutboxMessage => Set<OutboxMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
        modelBuilder.ApplyConfigurationsFromAssembly(AssemblyReference.Assembly);

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        await base.SaveChangesAsync(cancellationToken);
}