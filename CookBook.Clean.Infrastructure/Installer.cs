using CookBook.Clean.UseCases;
using Microsoft.Extensions.DependencyInjection;

namespace CookBook.Clean.Infrastructure;

public static class Installer
{
    public static IServiceCollection InstallInfraServices(this IServiceCollection services)
    {
        services.AddSingleton(typeof(IRepository<>), typeof(InMemoryRepository<>));
        
        return services;
    }
}