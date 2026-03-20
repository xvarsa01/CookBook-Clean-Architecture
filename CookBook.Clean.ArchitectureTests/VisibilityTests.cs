using ArchUnitNET.xUnit;
using MediatR;

namespace CookBook.Clean.ArchitectureTests;

using static ArchUnitNET.Fluent.ArchRuleDefinition;

public class VisibilityTests : ArchitectureTestBase
{
    [Fact]
    public void RequestHandlers_ShouldBeInternal()
    {
        Classes().That()
            .ImplementInterface(typeof(IRequestHandler<>))
            .Or()
            .ImplementInterface(typeof(IRequestHandler<,>))
            .Should().BeInternal()
            .Check(Architecture);
    }
}
