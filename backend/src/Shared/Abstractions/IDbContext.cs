using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Shared.Abstractions;

public interface IDbContext
{
    DatabaseFacade Database { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
