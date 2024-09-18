using MediatR;
using Shared.BuildingBlocks.Result;

namespace Shared.Abstractions.Messaging;

public interface ICommand : IRequest<Result>
{
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}
