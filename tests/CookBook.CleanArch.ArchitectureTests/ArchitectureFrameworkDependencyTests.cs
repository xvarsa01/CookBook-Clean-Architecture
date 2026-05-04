using ArchUnitNET.xUnit;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace CookBook.CleanArch.ArchitectureTests;

public class ArchitectureFrameworkDependencyTests : ArchitectureTestBase
{
    [Fact]
    public void CoreLayer_ShouldNotDependOn_EntityFramework()
    {
        Types().That().ResideInAssembly(DomainAssembly).Should()
            .NotDependOnAnyTypesThat()
            .ResideInNamespace("Microsoft.EntityFrameworkCore")
            .Check(Architecture);
    }
    
    [Fact]
    public void Presentation_WebApi_Layer_ShouldNotDependOn_EntityFramework()
    {
        Types().That().ResideInAssembly(WebApiAssembly).Should()
            .NotDependOnAnyTypesThat()
            .ResideInNamespace("Microsoft.EntityFrameworkCore")
            .Check(Architecture);
    }

    [Fact]
    public void CoreLayer_ShouldNotDependOn_AspNetCore()
    {
        Types().That().ResideInAssembly(DomainAssembly).Should()
            .NotDependOnAnyTypesThat()
            .ResideInNamespaceMatching("Microsoft.AspNetCore(\\..+)?")
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
