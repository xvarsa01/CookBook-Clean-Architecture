using CookBook.CleanArch.Domain.Shared;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace CookBook.CleanArch.Infrastructure.Outbox;

public class OutboxProcessor
{
    private const int BatchSize = 50;
    private readonly CookBookDbContext dbContext;
    private readonly IPublisher publisher;
    private readonly ILogger<OutboxProcessor> logger;

    public OutboxProcessor(CookBookDbContext dbContext, IPublisher publisher, ILogger<OutboxProcessor> logger)
    {
        this.dbContext = dbContext;
        this.publisher = publisher;
        this.logger = logger;
    }

    public async Task ProcessOutboxMessagesAsync(CancellationToken cancellationToken = default)
    {
        var messages = await dbContext.OutboxMessages
            .Where(x => x.DispatchedAt == null)
            .OrderBy(x => x.CreatedAt)
            .Take(BatchSize)
            .ToListAsync(cancellationToken);

        foreach (var message in messages)
        {
            try
            {
                var eventType = Type.GetType(message.Type);
                if (eventType == null)
                {
                    logger.LogWarning("Unknown outbox event type {EventType} for message {MessageId}", message.Type, message.Id);
                    continue;
                }

                var domainEvent = System.Text.Json.JsonSerializer.Deserialize(message.Content, eventType) as IDomainEvent;
                if (domainEvent == null)
                {
                    logger.LogWarning("Failed to deserialize outbox message {MessageId} to {EventType}", message.Id, eventType.Name);
                    continue;
                }

                await publisher.Publish(domainEvent, cancellationToken);

                message.DispatchedAt = DateTime.UtcNow;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error while dispatching outbox message {MessageId}", message.Id);
            }
        }

        if (dbContext.ChangeTracker.HasChanges())
        {
            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
