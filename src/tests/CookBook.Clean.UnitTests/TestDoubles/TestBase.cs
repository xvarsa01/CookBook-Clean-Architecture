using CookBook.Clean.Core;
using CookBook.Clean.Core.Shared;

namespace CookBook.Clean.UnitTests.TestDoubles;

public record TestBase : AggregateRootBase
{
    public override Guid Id { get; init; }
    public string Name { get; set; } = string.Empty;
}
