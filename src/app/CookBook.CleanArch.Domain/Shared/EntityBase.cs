namespace CookBook.CleanArch.Domain.Shared;

public abstract record EntityBase<TId> : IEntity<TId> where TId : StronglyTypedId
{
    public TId Id { get; init; } = null!;
    public DateTime CreatedAt { get; set; }

    public DateTime? ModifiedAt { get; set; }
}
