
using CookBook.CleanArch.Domain.Shared;

namespace CookBook.CleanArch.UnitTests.TestDoubles;

public sealed record TestBaseId(Guid Id) : StronglyTypedId(Id);

public record TestBase : AggregateRootBase<TestBaseId>
{
    public string Name { get; set; } = string.Empty;
}
