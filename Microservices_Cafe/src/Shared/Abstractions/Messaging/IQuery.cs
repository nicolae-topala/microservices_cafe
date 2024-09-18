using MediatR;
using Shared.BuildingBlocks.Result;

namespace Shared.Abstractions.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}

