using CookBook.Clean.Infrastructure.Factories;
using CookBook.Clean.Infrastructure.Repositories;
using CookBook.Clean.Application;
using CookBook.Clean.Application.ExternalInterfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CookBook.Clean.Infrastructure;

public static class Installer
{
    public static IServiceCollection AddInfraServices(this IServiceCollection services, DbOptions options)
    {
        if (options.UseInMemoryDb)
        {
            services.AddSingleton(typeof(IRepository<>), typeof(InMemoryRepositoryBase<>));
            services.AddSingleton<IRecipeRepository, InMemoryRecipeRepository>();
            return services;   
        }
        
        services.AddScoped(typeof(IRepository<>), typeof(EfRepositoryBase<>));
        services.AddScoped<IRecipeRepository, EfRecipeRepository>();
        
        services.AddSingleton<IDbContextFactory<CookBookDbContext>>(_ =>
            new DbContextSqLiteFactory(options.DatabaseFilePath, options.SeedDemoData));
        services.AddDbContext<CookBookDbContext>(contextOptions =>
            contextOptions.UseSqlite($"Data Source={options.DatabaseFilePath}"));
        
        services.AddScoped<DbContext>(provider => provider.GetRequiredService<CookBookDbContext>());
        
        return services;
    }
}
