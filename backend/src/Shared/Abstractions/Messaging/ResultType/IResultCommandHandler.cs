using MediatR;
using Shared.BuildingBlocks.Result;

namespace Shared.Abstractions.Messaging.ResultType;

public interface IResultCommandHandler<TCommand> : IRequestHandler<TCommand, Result>
    where TCommand : IResultCommand
{
}

public interface IResultCommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>> 
    where TCommand : IResultCommand<TResponse>
{
}
