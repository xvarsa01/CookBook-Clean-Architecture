namespace CookBook.CleanArch.Domain.Shared;

public interface IAggregateRoot
{
    IReadOnlyCollection<IDomainEvent> GetDomainEvents();
    void ClearDomainEvents();
}
