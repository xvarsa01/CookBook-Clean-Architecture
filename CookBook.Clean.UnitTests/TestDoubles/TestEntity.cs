using CookBook.Clean.Core;

namespace CookBook.Clean.UnitTests.TestDoubles;

public class TestEntity : IEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
