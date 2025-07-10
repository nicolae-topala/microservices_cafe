using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Shared.BuildingBlocks.Outbox;
using Shared.Primitives;

namespace Shared.BuildingBlocks.Interceptors;

public class ConvertDomainEventsToOutboxMessagesInterceptor(
    ILogger<ConvertDomainEventsToOutboxMessagesInterceptor> logger) 
    : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        DbContext? dbContext = eventData.Context;
        if (dbContext is null)
        {
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        logger.LogInformation("Interceptor called for context: {DbContext}", dbContext.GetType().Name);

        var aggregateRoots = dbContext.ChangeTracker
            .Entries<AggregateRoot>()
            .Select(x => x.Entity);

        var domainEvents = aggregateRoots
            .SelectMany(aggregateRoot =>
            {
                var domainEvents = aggregateRoot.GetDomainEvents().ToArray();
                aggregateRoot.ClearDomainEvents();

                return domainEvents; 
            });

        var outboxMessages = domainEvents
            .Select(domainEvent => new OutboxMessage
            {
                Id = Guid.NewGuid(),
                OccurredOnUtc = DateTime.UtcNow,
                Type = domainEvent.GetType().Name,
                EventType = EventType.Domain,
                Content = JsonConvert.SerializeObject(
                    domainEvent,
                    new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    })
            })
            .ToList();

        dbContext.Set<OutboxMessage>().AddRange(outboxMessages);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
