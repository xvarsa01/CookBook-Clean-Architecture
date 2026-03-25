using CookBook.CleanArch.Application.ExternalInterfaces;
using CookBook.CleanArch.Infrastructure.Factories;
using CookBook.CleanArch.Infrastructure.Interceptors;
using CookBook.CleanArch.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CookBook.CleanArch.Infrastructure;

public static class Installer
{
    public static IServiceCollection AddInfraServices(this IServiceCollection services, DbOptions options)
    {
        if (options.UseInMemoryDb)
        {
            services.AddSingleton(typeof(IRepository<,>), typeof(InMemoryRepository<,>));
            return services;   
        }
        
        services.AddScoped(typeof(IRepository<,>), typeof(EfRepository<,>));
        
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
