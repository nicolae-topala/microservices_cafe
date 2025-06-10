using Microsoft.EntityFrameworkCore;
using Shared.BuildingBlocks.Outbox;

namespace Shared.Abstractions;

public interface IHasOutboxMessages
{
    DbSet<OutboxMessage> OutboxMessage { get; }
}