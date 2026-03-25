using System.Windows.Input;
using ArchUnitNET.xUnit;
using CookBook.Clean.Application.Abstraction;
using MediatR;
using static ArchUnitNET.Fluent.ArchRuleDefinition;

namespace CookBook.Clean.ArchitectureTests
{
    public class ArchitectureApplicationTests : ArchitectureTestBase
    {
        [Fact]
        public void Commands_Should_Not_HaveDependencyOnAnything()
        {
            var rule = Classes()
                .That()
                .HaveNameEndingWith("Command")
                .Should()
                .NotDependOnAny();

            rule.Check(Architecture);
        }

        [Fact]
        public void Commands_Should_Implement_ICommand()
        {
            Classes()
                .That()
                .HaveNameEndingWith("Command")
                .Should()
                .ImplementInterface(typeof(ICommandBase))
                .Check(Architecture);
        }
        
        [Fact]
        public void CommandHandlers_Should_HaveDependencyOnCoreProject()
        {
            var coreTypes = Types()
                .That()
                .ResideInNamespaceMatching(CoreNamespace);
            
            var rule = Classes()
                .That()
                .HaveNameEndingWith("CommandHandler")
                .Should()
                .DependOnAny(coreTypes);

            rule.Check(Architecture);
        }
        
        [Fact]
        public void CommandHandlers_Should_Not_HaveDependencyOnRepositories()
        {
            var handlers = Classes()
                .That()
                .HaveNameEndingWith("CommandHandler");

            var forbiddenClasses = Classes()
                .That()
                .HaveNameEndingWith("Repository");
            
            handlers
                .Should()
                .NotDependOnAny(forbiddenClasses)
                .Check(Architecture);
        }
        
        [Fact]
        public void CommandHandler_Should_HaveDependencyOnIRepositories()
        {
            var handlers = Classes()
                .That()
                .ImplementInterface(typeof(ICommandHandler<>))
                .Or().ImplementInterface(typeof(ICommandHandler<,>));

            // Include the generic interface directly
            var allowedTypes = Types()
                .That()
                .Are(typeof(Application.ExternalInterfaces.IRepository<,>));

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
                .HaveNameContaining("Repository")
                .Should()
                .ImplementInterface(typeof(Application.ExternalInterfaces.IRepository<,>))
                .Check(Architecture);
        }
    }
}
