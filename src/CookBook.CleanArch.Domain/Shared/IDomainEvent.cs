using MediatR;

namespace CookBook.CleanArch.Domain.Shared;

public interface IDomainEvent : INotification
{
    public Guid Id { get; }
    public DateTime CreatedAt { get; }
}
