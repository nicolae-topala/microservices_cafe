using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Price.Domain.Entities;

namespace Price.Application.Abstractions;

public interface IPriceDbContext
{
    DbSet<Channel> Channels { get; }
    DbSet<DiscountRule> DiscountRules { get; }
    DbSet<ProductPrice> ProductPrices { get; }
    DatabaseFacade Database { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
