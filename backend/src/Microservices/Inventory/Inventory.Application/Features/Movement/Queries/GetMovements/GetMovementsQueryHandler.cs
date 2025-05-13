using Inventory.Application.Abstractions;
using Microsoft.EntityFrameworkCore;
using Shared.Abstractions.Messaging;
using MovementDomain = Inventory.Domain.Entities.Movement;

namespace Inventory.Application.Features.Movement.Queries.GetMovements;
public class GetMovementsQueryHandler(IInventoryDbContext dbContext)
    : IQueryHandler<GetMovementsQuery, IQueryable<MovementDomain>>
{
    public Task<IQueryable<MovementDomain>> Handle(GetMovementsQuery request, CancellationToken cancellationToken) =>
        Task.FromResult(dbContext.Movements
            .Include(m => m.Item)
                .ThenInclude(i => i.Location)
            .Include(m => m.Location)
                .ThenInclude(l => l.LocationType)
            .Include(m => m.MovementType)
            .AsNoTracking());
}
