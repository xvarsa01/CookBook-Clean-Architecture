using CookBook.Clean.Core;

namespace CookBook.Clean.UnitTests.TestDoubles;

public class TestEntity : IAggregateRootEntity
{
    public Guid Id { get; init; }
    public string Name { get; set; } = string.Empty;
}
