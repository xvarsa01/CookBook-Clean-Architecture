namespace CookBook.CleanArch.Domain.Shared;

public abstract record EntityBase<TId>(TId Id) : IEntity<TId> where TId : StronglyTypedId
{
    public DateTime CreatedAt { get; set; }

    public DateTime? ModifiedAt { get; set; }
}
