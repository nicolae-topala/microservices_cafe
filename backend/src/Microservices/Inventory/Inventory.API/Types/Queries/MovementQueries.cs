using Inventory.Application.Features.Movement.Queries.GetMovements;
using Inventory.Domain.Entities;
using MediatR;

namespace Inventory.API.Types.Queries;

[QueryType]
public class MovementQueries
{
    [UsePaging]
    [UseFiltering]
    [UseSorting]
    public Task<IQueryable<Movement>> GetMovements(ISender sender, CancellationToken cancellationToken) =>
        sender.Send(new GetMovementsQuery(), cancellationToken);
}
