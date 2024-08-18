using MediatR;
using MicroservicesCafe.Shared.BuildingBlocks.Result;

namespace MicroservicesCafe.Shared.Abstractions.Messaging;

public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
}
