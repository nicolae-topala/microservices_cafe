using Newtonsoft.Json;

namespace Shared.BuildingBlocks.Outbox;

public enum EventType
{
    Domain,
    Integration
}

public class OutboxMessage
{
    public Guid Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public EventType EventType { get; set; } = EventType.Domain;
    public DateTime OccurredOnUtc { get; set; }
    public DateTime? ProcessedOnUtc { get; set; }
    public string? Error { get; set; }
    
    public OutboxMessage() { }
    
    private OutboxMessage(object integrationEvent, string typeName)
    {
        Id = Guid.NewGuid();
        OccurredOnUtc = DateTime.UtcNow;
        Type = typeName;
        EventType = EventType.Integration;
        Content = JsonConvert.SerializeObject(
            integrationEvent,
            new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });
    }

    public static OutboxMessage CreateIntegrationEventOutboxMessage(object integrationEvent, string typeName) =>
        new(integrationEvent, typeName);
}