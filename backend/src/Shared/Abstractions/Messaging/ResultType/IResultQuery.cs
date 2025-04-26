using MediatR;
using Shared.BuildingBlocks.Result;

namespace Shared.Abstractions.Messaging.ResultType;

public interface IResultQuery<TResponse> : IRequest<Result<TResponse>>, IQueryBase
{
}

