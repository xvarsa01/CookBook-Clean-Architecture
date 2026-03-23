using ArchUnitNET.xUnit;
using CookBook.Clean.Application.Filters;
using CookBook.Clean.Application.Mappers;
using CookBook.Clean.Application.Models;
using MediatR;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace CookBook.Clean.ArchitectureTests;

public class ArchitectureNamingConventionTests : ArchitectureTestBase
{
    [Fact]
    public void CommandHandlers_ShouldHave_NameEndingWith_CommandHandler()
    {
        Classes().That()
            .ImplementInterface(typeof(IRequestHandler<>))
            .Or()
            .ImplementInterface(typeof(IRequestHandler<,>))
            .Should().HaveNameEndingWith("Handler")
            .Check(Architecture);
    }
    
    [Fact]
    public void UseCases_ShouldHave_NameEndingWith_UseCase()
    {
        Classes().That()
            .ImplementInterface(typeof(IRequest<>))
            .Should().HaveNameMatching(".*(UseCase|Query)$")
            .Check(Architecture);
    }
    
    [Fact]
    public void Events_ShouldHave_NameEndingWith_Event()
    {
        Classes().That()
            .ImplementInterface(typeof(INotification))
            .Should().HaveNameEndingWith("Event")
            .Check(Architecture);
    }
    
    [Fact]
    public void Filters_ShouldHave_NameEndingWith_Filter()
    {
        Classes().That()
            .ImplementInterface(typeof(IFilter<>))
            .Should().HaveNameMatching("Filter")
            .Check(Architecture);
    }
    
    [Fact]
    public void Mappers_ShouldHave_NameEndingWith_Mapper()
    {
        Classes().That()
            .ImplementInterface(typeof(IMapper))
            .Should().HaveNameMatching("Mapper")
            .Check(Architecture);
    }
    
    [Fact]
    public void Models_ShouldHave_NameEndingWith_Model()
    {
        Classes().That()
            .ImplementInterface(typeof(IModel))
            .Should().HaveNameMatching("Model")
            .Check(Architecture);
    }
    
    
    
    [Fact]
    public void Entities_ShouldBeIn_Core_Layer()
    {
        Classes().That()
            .HaveNameEndingWith("Entity")
            .Should().ResideInAssembly(CoreAssembly)
            .Check(Architecture);
    }
    
    [Fact]
    public void UseCases_ShouldBeIn_Application_Layer()
    {
        Classes().That()
            .HaveNameEndingWith("UseCase")
            .Should().ResideInAssembly(ApplicationAssembly)
            .Check(Architecture);
    }
    
    [Fact]
    public void Queries_ShouldBeIn_Application_Layer()
    {
        Classes().That()
            .HaveNameEndingWith("Query")
            .Should().ResideInAssembly(ApplicationAssembly)
            .Check(Architecture);
    }
    
    [Fact]
    public void Handlers_ShouldBeIn_Application_Layer()
    {
        Classes().That()
            .HaveNameEndingWith("Handler")
            .Should().ResideInAssembly(ApplicationAssembly)
            .Check(Architecture);
    }
    
    [Fact]
    public void EventHandlers_ShouldBeIn_Application_Layer()
    {
        Classes().That()
            .HaveNameEndingWith("EventHandler")
            .Should().ResideInAssembly(ApplicationAssembly)
            .Check(Architecture);
    }
    
    [Fact]
    public void Filters_ShouldBeIn_Application_Layer()
    {
        Classes().That()
            .HaveNameEndingWith("Filter")
            .Should().ResideInAssembly(ApplicationAssembly)
            .Check(Architecture);
    }
    
    [Fact]
    public void Repositories_ShouldBeIn_Infrastructure_Layer()
    {
        Classes().That()
            .HaveNameEndingWith("Repository")
            .Should().ResideInAssembly(InfrastructureAssembly)
            .Check(Architecture);
    }

    [Fact]
    public void RepositoryInterfaces_ShouldBeIn_Application_Layer()
    {
        Interfaces().That()
            .HaveNameEndingWith("Repository")
            .Should().ResideInAssembly(ApplicationAssembly)
            .Check(Architecture);
    }
    
    [Fact]
    public void Factories_ShouldBeIn_Infrastructure_Layer()
    {
        Classes().That()
            .HaveNameEndingWith("Factory")
            .Should().ResideInAssembly(InfrastructureAssembly)
            .Check(Architecture);
    }
    
    [Fact]
    public void DTOs_ShouldBeIn_WebApi_Layer()
    {
        Classes().That()
            .HaveNameEndingWith("DTO")
            .Or().HaveNameEndingWith("Dto")
            .Should().ResideInAssembly(WebApiAssembly)
            .Check(Architecture);
    }

    [Fact]
    public void Controllers_ShouldBeIn_WebApi_Layer()
    {
        Classes().That()
            .HaveNameEndingWith("Controller")
            .Should().ResideInAssembly(WebApiAssembly)
            .Check(Architecture);
    }

    [Fact]
    public void RootEntities_ShouldBeIn_Core_Layer()
    {
        Classes().That()
            .ImplementInterface(typeof(Core.IRootEntity))
            .Should().ResideInAssembly(CoreAssembly)
            .Check(Architecture);
    }
}
