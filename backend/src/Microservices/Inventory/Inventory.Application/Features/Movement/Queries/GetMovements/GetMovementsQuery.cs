using Shared.Abstractions.Messaging;
using MovementDomain = Inventory.Domain.Entities.Movement;

namespace Inventory.Application.Features.Movement.Queries.GetMovements;

public record GetMovementsQuery : IQuery<IQueryable<MovementDomain>>;
