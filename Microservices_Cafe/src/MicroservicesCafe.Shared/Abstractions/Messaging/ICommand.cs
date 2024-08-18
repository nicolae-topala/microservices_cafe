using MediatR;
using MicroservicesCafe.Shared.BuildingBlocks.Result;

namespace MicroservicesCafe.Shared.Abstractions.Messaging;

public interface ICommand : IRequest<Result>
{
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}
