using MediatR;
using Shared.Abstractions;
using Shared.BuildingBlocks.Outbox;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Quartz;

namespace Shared.BuildingBlocks.BackgroundJobs;

[DisallowConcurrentExecution]
public class ProcessOutboxMessagesJob<TContext> : IJob where TContext : DbContext
{
    private readonly TContext _dbContext;
    private readonly IPublisher _publisher;
    private const int batchSize = 20;

    public ProcessOutboxMessagesJob(TContext dbContext, IPublisher publisher)
    {
        _dbContext = dbContext;
        _publisher = publisher;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        List<OutboxMessage> messages = await _dbContext
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

            await _publisher.Publish(domainEvent, context.CancellationToken).ConfigureAwait(false);

            outboxMessage.ProcessedOnUtc = DateTime.UtcNow;
        }

        await _dbContext.SaveChangesAsync(context.CancellationToken).ConfigureAwait(false);
    }
}
