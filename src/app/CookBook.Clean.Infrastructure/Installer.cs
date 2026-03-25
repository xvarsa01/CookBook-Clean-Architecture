using CookBook.Clean.Infrastructure.Factories;
using CookBook.Clean.Infrastructure.Repositories;
using CookBook.Clean.Application.ExternalInterfaces;
using CookBook.Clean.Infrastructure.Interceptors;
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
            return services;   
        }
        
        services.AddScoped(typeof(IRepository<>), typeof(EfRepositoryBase<>));
        
        services.AddSingleton<IDbContextFactory<CookBookDbContext>>(_ =>
            new DbContextSqLiteFactory(options.DatabaseFilePath, options.SeedDemoData));
        services.AddDbContext<CookBookDbContext>((sp, contextOptions) =>
        {
            contextOptions.UseSqlite($"Data Source={options.DatabaseFilePath}");
            contextOptions.AddCreatedDateUpdatedDateInterceptor(sp);
        });
        
        services.AddScoped<DbContext>(provider => provider.GetRequiredService<CookBookDbContext>());
        services.AddSingleton<CreatedDateUpdatedDateInterceptor>();
        
        return services;
    }
    
    private static void AddCreatedDateUpdatedDateInterceptor(this DbContextOptionsBuilder dbContextOptionsBuilder,
        IServiceProvider serviceProvider)
    {
        var interceptor = serviceProvider.GetService<CreatedDateUpdatedDateInterceptor>();

        if (interceptor != null)
        {
            dbContextOptionsBuilder.AddInterceptors(interceptor);
        }
    }
}
