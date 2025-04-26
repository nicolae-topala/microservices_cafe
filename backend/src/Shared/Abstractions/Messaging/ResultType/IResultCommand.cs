using MediatR;
using Shared.BuildingBlocks.Result;

namespace Shared.Abstractions.Messaging.ResultType;

public interface IResultCommand : IRequest<Result>, ICommandBase
{
}

public interface IResultCommand<TResponse> : IRequest<Result<TResponse>>, ICommandBase
{
}
