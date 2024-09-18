﻿using MediatR;
using Shared.BuildingBlocks.Result;

namespace Shared.Abstractions.Messaging;

public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, Result> 
    where TCommand : ICommand
{
}

public interface ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>> 
    where TCommand : ICommand<TResponse>
{
}
