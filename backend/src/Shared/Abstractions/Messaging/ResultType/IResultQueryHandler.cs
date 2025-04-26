using MediatR;
using Shared.BuildingBlocks.Result;

namespace Shared.Abstractions.Messaging.ResultType;

public interface IResultQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IResultQuery<TResponse>
{
}
