using ArchUnitNET.xUnit;
using MediatR;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace CookBook.Clean.ArchitectureTests
{
    public class ArchitectureApplicationTests : ArchitectureTestBase
    {
        [Fact]
        public void Application_UseCases_Should_Not_HaveDependencyOnAnything()
        {
            var rule = Classes()
                .That()
                .HaveNameEndingWith("UseCase")
                .Should()
                .NotDependOnAny();

            rule.Check(Architecture);
        }

        [Fact]
        public void UseCases_And_Queries_Should_Implement_MediatR_IRequest()
        {
            Classes()
                .That()
                .HaveNameEndingWith("UseCase")
                .Or()
                .HaveNameEndingWith("Query")
                .Should()
                .ImplementInterface(typeof(IRequest<>))
                .Check(Architecture);
        }
        
        [Fact]
        public void Application_Handlers_Should_HaveDependencyOnCoreProject()
        {
            var coreTypes = Types()
                .That()
                .ResideInNamespaceMatching(CoreNamespace);
            
            var rule = Classes()
                .That()
                .HaveNameEndingWith("Handler")
                .Should()
                .DependOnAny(coreTypes);

            rule.Check(Architecture);
        }

        [Fact]
        public void UseCase_Should_Not_HaveDependencyOnRepositories()
        {
            var useCaseOrQueryTypes = Classes()
                .That()
                .HaveNameEndingWith("UseCase");

            var forbiddenRepoTypes = Types()
                .That()
                .HaveNameEndingWith("Repository");
            
            useCaseOrQueryTypes
                .Should()
                .NotDependOnAny(forbiddenRepoTypes)
                .Check(Architecture);
        }
        
        [Fact]
        public void Handlers_Should_Not_HaveDependencyOnRepositories()
        {
            var handlers = Classes()
                .That()
                .HaveNameEndingWith("Handler");

            var forbiddenClasses = Classes()
                .That()
                .HaveNameEndingWith("Repository");
            
            handlers
                .Should()
                .NotDependOnAny(forbiddenClasses)
                .Check(Architecture);
        }
        
        [Fact]
        public void Handlers_Should_HaveDependencyOnIRepositories()
        {
            var handlers = Classes()
                .That()
                .ImplementInterface(typeof(IRequestHandler<>))
                .Or().ImplementInterface(typeof(IRequestHandler<,>));

            // Include the generic interface directly
            var allowedTypes = Types()
                .That()
                .Are(typeof(Application.ExternalInterfaces.IRepository<>));

            handlers
                .Should()
                .DependOnAny(allowedTypes)
                .Check(Architecture);
        }

        [Fact]
        public void EventHandlers_Should_Implement_NotificationHandler()
        {
            Classes()
                .That()
                .HaveNameEndingWith("EventHandler")
                .Should()
                .ImplementInterface(typeof(INotificationHandler<>))
                .Check(Architecture);
        }

        [Fact]
        public void Repository_Implementations_Should_Implement_IRepository()
        {
            Classes()
                .That()
                .HaveNameEndingWith("Repository")
                .Should()
                .ImplementInterface(typeof(Application.ExternalInterfaces.IRepository<>))
                .Check(Architecture);
        }
    }
}
