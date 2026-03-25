using ArchUnitNET.Domain;
using ArchUnitNET.Loader;
using CookBook.Clean.Core.Shared;
using Assembly = System.Reflection.Assembly;

namespace CookBook.Clean.ArchitectureTests;

public class ArchitectureTestBase
{
    protected const string CoreNamespace = "CookBook.Clean.Core";
    protected const string ApplicationNamespace = "CookBook.Clean.Application";
    protected const string InfrastructureNamespace = "CookBook.Clean.Infrastructure";
    protected const string MauiNamespace = "CookBook.Clean.Ui";
    protected const string WebApiNamespace = "CookBook.Clean.WebApi";
    
    protected static readonly Assembly CoreAssembly = typeof(AggregateRootBase<>).Assembly;
    protected static readonly Assembly ApplicationAssembly = typeof(Application.Installer).Assembly;
    protected static readonly Assembly InfrastructureAssembly = typeof(Infrastructure.Installer).Assembly;
    protected static readonly Assembly WebApiAssembly = typeof(Program).Assembly;

    protected static readonly Architecture Architecture = new ArchLoader()
        .LoadAssemblies(
            CoreAssembly,
            ApplicationAssembly,
            InfrastructureAssembly,
            WebApiAssembly)
        .Build();
}
