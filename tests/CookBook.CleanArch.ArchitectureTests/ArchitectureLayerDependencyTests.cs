using ArchUnitNET.Domain;
using ArchUnitNET.xUnit;
using MediatR;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace CookBook.CleanArch.ArchitectureTests;

public class ArchitectureLayerDependencyTests : ArchitectureTestBase
{
    private static readonly IObjectProvider<IType> DomainLayer =
        Types().That().ResideInAssembly(DomainAssembly).As("Domain layer");

    private static readonly IObjectProvider<IType> ApplicationLayer =
        Types().That().ResideInAssembly(ApplicationAssembly).As("Application layer");

    private static readonly IObjectProvider<IType> InfrastructureLayer =
        Types().That().ResideInAssembly(InfrastructureAssembly).As("Infrastructure Layer");

    private static readonly IObjectProvider<IType> PresentationLayer =
        Types().That().ResideInAssembly(WebApiAssembly).As("Presentation Layer");

    [Fact]
    public void DomainLayer_ShouldNotDependOn_ApplicationLayer()
    {
        Types().That().Are(DomainLayer).Should()
            .NotDependOnAny(ApplicationLayer)
            .Check(Architecture);
    }

    [Fact]
    public void DomainLayer_ShouldNotDependOn_InfrastructureLayer()
    {
        Types().That().Are(DomainLayer).Should()
            .NotDependOnAny(InfrastructureLayer)
            .Check(Architecture);
    }

    [Fact]
    public void DomainLayer_ShouldNotDependOn_PresentationLayer()
    {
        Types().That().Are(DomainLayer).Should()
            .NotDependOnAny(PresentationLayer)
            .Check(Architecture);
    }

    [Fact]
    public void ApplicationLayer_ShouldNotDependOn_InfrastructureLayer()
    {
        Types().That().Are(ApplicationLayer).Should()
            .NotDependOnAny(InfrastructureLayer)
            .Check(Architecture);
    }

    [Fact]
    public void ApplicationLayer_ShouldNotDependOn_PresentationLayer()
    {
        Types().That().Are(ApplicationLayer).Should()
            .NotDependOnAny(PresentationLayer)
            .Check(Architecture);
    }

    [Fact]
    public void InfrastructureLayer_ShouldNotDependOn_PresentationLayer()
    {
        Types().That().Are(InfrastructureLayer).Should()
            .NotDependOnAny(PresentationLayer)
            .Check(Architecture);
    }
    
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
}
