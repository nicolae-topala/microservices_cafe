using Inventory.Shared.DTOs.Location;
using Shared.Abstractions.Messaging.ResultType;
using LocationDomain = Inventory.Domain.Entities.Location;

namespace Inventory.Application.Features.Location.Commands.CreateLocation;

public record CreateLocationCommand(CreateLocationDto Location) : IResultCommand<LocationDomain>;