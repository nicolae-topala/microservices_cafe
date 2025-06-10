using MediatR;
using Shared.Abstractions;
using Shared.BuildingBlocks.Outbox;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Quartz;
using MassTransit;

namespace Shared.BuildingBlocks.BackgroundJobs;

[DisallowConcurrentExecution]
public class ProcessOutboxMessagesJob<TContext>(
    TContext dbContext,
    IPublisher publisher,
    IPublishEndpoint publishEndpoint) 
    : IJob
    where TContext : IDbContext, IHasOutboxMessages
{
    private const int _batchSize = 20;

    public async Task Execute(IJobExecutionContext context)
    {
        List<OutboxMessage> messages = await dbContext.OutboxMessage
            .Where(m => m.ProcessedOnUtc == null)
            .Take(_batchSize)
            .ToListAsync(context.CancellationToken)
            .ConfigureAwait(false);

        foreach (OutboxMessage outboxMessage in messages)
        {
            await HandlePublishingAsync(outboxMessage, context.CancellationToken).ConfigureAwait(false);
        }

        await dbContext.SaveChangesAsync(context.CancellationToken).ConfigureAwait(false);
    }

    private async Task HandlePublishingAsync(OutboxMessage outboxMessage, CancellationToken cancellationToken)
    {
        try
        {
            switch (outboxMessage.EventType)
            {
                case EventType.Domain:
                    await ProcessDomainEventAsync(outboxMessage, cancellationToken).ConfigureAwait(false);
                    break;

                case EventType.Integration:
                    await ProcessIntegrationEventAsync(outboxMessage, cancellationToken).ConfigureAwait(false);
                    break;
            }

            outboxMessage.ProcessedOnUtc = DateTime.UtcNow;
        }
        catch (Exception ex)
        {
            outboxMessage.Error = ex.ToString();
        }
    }

    private async Task ProcessDomainEventAsync(OutboxMessage outboxMessage, CancellationToken cancellationToken)
    {
        IDomainEvent? domainEvent = JsonConvert.DeserializeObject<IDomainEvent>(
            outboxMessage.Content,
            new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });

        if (domainEvent is not null)
        {
            await publisher.Publish(domainEvent, cancellationToken).ConfigureAwait(false);
        }
    }

    private async Task ProcessIntegrationEventAsync(OutboxMessage outboxMessage, CancellationToken cancellationToken)
    {
        var messageType = AssemblyReference.Assembly.GetType(outboxMessage.Type)!;
        var integrationEvent = JsonConvert.DeserializeObject(outboxMessage.Content, messageType);

        if (integrationEvent is not null)
        {
            await publishEndpoint.Publish(integrationEvent, messageType, cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
