using ArchUnitNET.xUnit;
using MediatR;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace CookBook.CleanArch.ArchitectureTests;

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

        [Fact]
        public void WebApi_Controllers_Should_NotDependOnInfrastructure()
        {
            Classes()
                .That()
                .HaveNameEndingWith("Controller")
                .Should()
                .NotDependOnAnyTypesThat()
                .ResideInNamespaceMatching("CookBook.Clean.Infrastructure(\\..+)?")
                .Check(Architecture);
        }

        [Fact]
        public void WebApi_ShouldNotDependOn_EntityFramework()
        {
            Types().That().ResideInAssembly(WebApiAssembly).Should()
                .NotDependOnAnyTypesThat()
                .ResideInNamespace("Microsoft.EntityFrameworkCore")
                .Check(Architecture);
        }
}
