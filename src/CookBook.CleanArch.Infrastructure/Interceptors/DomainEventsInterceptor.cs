using System.Text.Json;
using CookBook.CleanArch.Domain.Shared;
using CookBook.CleanArch.Infrastructure.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CookBook.CleanArch.Infrastructure.Interceptors;

public sealed class DomainEventsInterceptor() : SaveChangesInterceptor
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

        var outboxMessages = dbContext.ChangeTracker
              .Entries<IAggregateRoot>()
              .Select(x => x.Entity)
              .SelectMany(aggregateRoot =>
              {
                  var domainEvents = aggregateRoot.GetDomainEvents();

                  aggregateRoot.ClearDomainEvents();

                  return domainEvents;
              })
              .
              Select(domainEvent => new OutboxMessage
              {
                  Id = Guid.NewGuid(),
                  Type = domainEvent.GetType().AssemblyQualifiedName!,
                  Content = JsonSerializer.Serialize(domainEvent, domainEvent.GetType()),
                  CreatedAt = DateTime.UtcNow
              })
              .ToList();

        dbContext.Set<OutboxMessage>().AddRange(outboxMessages);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
