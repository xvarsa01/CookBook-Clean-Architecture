using MediatR;

namespace CookBook.CleanArch.Domain.Shared;

public interface IDomainEvent : INotification
{
    Guid Id { get; }
    DateTime CreatedAt { get; }
}
