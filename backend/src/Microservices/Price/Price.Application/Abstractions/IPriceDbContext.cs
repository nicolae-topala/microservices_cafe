using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Price.Domain.Entities;
using Shared.Abstractions;

namespace Price.Application.Abstractions;

public interface IPriceDbContext : IHasOutboxMessages, IDbContext
{
    DbSet<Channel> Channels { get; }
    DbSet<DiscountRule> DiscountRules { get; }
    DbSet<ProductPrice> ProductPrices { get; }
}
