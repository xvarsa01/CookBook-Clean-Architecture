namespace CookBook.CleanArch.Domain.Shared;

public abstract record AggregateRootBase<TId>(TId Id) : EntityBase<TId>(Id), IAggregateRoot where TId : StronglyTypedId
{
    private readonly List<IDomainEvent> _domainEvents = [];
    public IReadOnlyCollection<IDomainEvent> GetDomainEvents() => _domainEvents.ToList().AsReadOnly();
    public void ClearDomainEvents() => _domainEvents.Clear();
    protected void RaiseEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
}
