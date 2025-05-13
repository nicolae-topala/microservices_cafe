using Inventory.Shared.DTOs.Location;
using Shared.Abstractions.Messaging.ResultType;

namespace Inventory.Application.Features.Location.Commands.UpdateLocation;

public record UpdateLocationCommand(UpdateLocationDto Location) : IResultCommand;
