namespace CookBook.Clean.Core.Shared;

public interface IEntity
{
    Guid Id { get; init; }
    DateTime CreatedAt { get; set; }

    DateTime? ModifiedAt { get; set; }
}
