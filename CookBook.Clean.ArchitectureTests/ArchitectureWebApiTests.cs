using ArchUnitNET.xUnit;
using MediatR;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace CookBook.Clean.ArchitectureTests;

public class ArchitectureWebApiTests : ArchitectureTestBase
{
        [Fact]
        public void WebApi_Controllers_Should_HaveDependencyOnMediatR()
        {
            var rule = Classes()
                .That()
                .HaveNameEndingWith("Controller")
                .Should()
                .DependOnAny(typeof(IMediator));

            rule.Check(Architecture);
        }
}
