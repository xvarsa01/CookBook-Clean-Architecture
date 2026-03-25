using CookBook.Clean.Core.Shared;

namespace CookBook.Clean.UnitTests.TestDoubles;

public sealed record TestBaseId(Guid Id) : StronglyTypedId(Id);

public record TestBase : AggregateRootBase<TestBaseId>
{
    public string Name { get; set; } = string.Empty;
}
