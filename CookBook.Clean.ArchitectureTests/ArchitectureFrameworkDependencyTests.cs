using ArchUnitNET.xUnit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace CookBook.Clean.ArchitectureTests;

public class ArchitectureFrameworkDependencyTests : ArchitectureTestBase
{
    [Fact]
    public void CoreLayer_ShouldNotDependOn_EntityFramework()
    {
        Types().That().ResideInAssembly(CoreAssembly).Should()
            .NotDependOnAnyTypesThat()
            .ResideInNamespace("Microsoft.EntityFrameworkCore")
            .Check(Architecture);
    }

    [Fact]
    public void CoreLayer_ShouldNotDependOn_AspNetCore()
    {
        Types().That().ResideInAssembly(CoreAssembly).Should()
            .NotDependOnAnyTypesThat()
            .ResideInNamespaceMatching("Microsoft.AspNetCore(\\..+)?")
            .Check(Architecture);
    }

    [Fact]
    public void ApplicationLayer_ShouldNotDependOn_EntityFramework()
    {
        Types().That().ResideInAssembly(ApplicationAssembly).Should()
            .NotDependOnAnyTypesThat()
            .ResideInNamespace("Microsoft.EntityFrameworkCore")
            .Check(Architecture);
    }

    [Fact]
    public void ApplicationLayer_ShouldNotDependOn_AspNetCore()
    {
        Types().That().ResideInAssembly(ApplicationAssembly).Should()
            .NotDependOnAnyTypesThat()
            .ResideInNamespaceMatching("Microsoft.AspNetCore(\\..+)?")
            .Check(Architecture);
    }
}
