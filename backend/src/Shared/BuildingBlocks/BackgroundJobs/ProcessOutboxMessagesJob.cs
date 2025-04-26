using MediatR;
using Shared.Abstractions;
using Shared.BuildingBlocks.Outbox;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Quartz;

namespace Shared.BuildingBlocks.BackgroundJobs;

[DisallowConcurrentExecution]
public class ProcessOutboxMessagesJob<TContext>(
    TContext dbContext,
    IPublisher publisher) 
    : IJob
    where TContext : DbContext
{
    private const int batchSize = 20;

    public async Task Execute(IJobExecutionContext context)
    {
        List<OutboxMessage> messages = await dbContext
            .Set<OutboxMessage>()
            .Where(m => m.ProcessedOnUtc == null)
            .Take(batchSize)
            .ToListAsync(context.CancellationToken)
            .ConfigureAwait(false);

        foreach (OutboxMessage outboxMessage in messages)
        {
            IDomainEvent? domainEvent = JsonConvert
                .DeserializeObject<IDomainEvent>(
                    outboxMessage.Content,
                    new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    });

            if (domainEvent is null)
            {
                continue;
            }

            await publisher.Publish(domainEvent, context.CancellationToken).ConfigureAwait(false);

            outboxMessage.ProcessedOnUtc = DateTime.UtcNow;
        }

        await dbContext.SaveChangesAsync(context.CancellationToken).ConfigureAwait(false);
    }
}
