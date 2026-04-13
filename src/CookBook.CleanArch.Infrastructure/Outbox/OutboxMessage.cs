namespace CookBook.CleanArch.Infrastructure.Outbox;

public class OutboxMessage
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Type { get; set; }           // Event type (e.g. "OrderCreated")
    public required string Content { get; set; }        // JSON payload
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? DispatchedAt { get; set; } // null = not yet sent
}
