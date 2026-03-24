namespace CookBook.Clean.Core.Shared;

public abstract record EntityBase : IEntity
{
    public abstract Guid Id { get; init; }
    public DateTime CreatedAt { get; set; }

    public DateTime? ModifiedAt { get; set; }
}
