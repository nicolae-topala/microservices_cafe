using Shared.Abstractions;

namespace Shared.BuildingBlocks.Outbox;

public static class IntegrationEventOutboxHelper
{
    public static void AddIntegrationEvent<TContext, TIntegrationEvent>(
        this TContext dbContext, 
        TIntegrationEvent integrationEvent)
        where TIntegrationEvent : IIntegrationEvent
        where TContext : IHasOutboxMessages
    {
        var outboxMessage = OutboxMessage.CreateIntegrationEventOutboxMessage(
            integrationEvent,
            integrationEvent.GetType().FullName!);

        dbContext.OutboxMessage.Add(outboxMessage);
    }
}