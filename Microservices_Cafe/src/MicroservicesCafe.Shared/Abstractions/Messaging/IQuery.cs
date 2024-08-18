using MediatR;
using MicroservicesCafe.Shared.BuildingBlocks.Result;

namespace MicroservicesCafe.Shared.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}

