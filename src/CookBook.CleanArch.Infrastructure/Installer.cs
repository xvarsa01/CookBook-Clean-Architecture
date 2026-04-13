using CookBook.CleanArch.Application.ExternalInterfaces;
using CookBook.CleanArch.Domain.Recipe;
using CookBook.CleanArch.Domain.Recipe.ValueObjects;
using CookBook.CleanArch.Infrastructure.Email;
using CookBook.CleanArch.Infrastructure.Factories;
using CookBook.CleanArch.Infrastructure.Interceptors;
using CookBook.CleanArch.Infrastructure.Outbox;
using CookBook.CleanArch.Infrastructure.Migrator;
using CookBook.CleanArch.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace CookBook.CleanArch.Infrastructure;

public static class Installer
{
    public static IServiceCollection AddInfraServices(this IServiceCollection services, DbOptions options)
    {
        services.AddSingleton(Options.Create(options));

        if (options.UseInMemoryDb)
        {
            services.AddSingleton(typeof(IRepository<,>), typeof(InMemoryRepository<,>));
            return services;   
        }
        
        services.AddScoped<IDbMigrator, DbMigrator>();
        services.AddScoped<IDbSeeder, DbSeeder>();
        services.AddScoped(typeof(IRepository<,>), typeof(EfRepository<,>));
        services.AddScoped(typeof(IRepository<Recipe, RecipeId>), typeof(EfRecipeRepository));
        
        services.AddSingleton<IDbContextFactory<CookBookDbContext>>(_ =>
            new DbContextSqLiteFactory(options.DatabaseFilePath));
        services.AddDbContext<CookBookDbContext>((sp, contextOptions) =>
        {
            contextOptions.UseSqlite($"Data Source={options.DatabaseFilePath}");
            contextOptions.AddCreatedDateUpdatedDateInterceptor(sp);
            contextOptions.AddDomainsEventInterceptor(sp);
        });
        
        services.AddScoped<ICookBookDbContext>(sp => sp.GetRequiredService<CookBookDbContext>());
        services.AddScoped<DbContext>(provider => provider.GetRequiredService<CookBookDbContext>());
        services.AddScoped<OutboxProcessor>();
        services.AddHostedService<OutboxBackgroundService>();
        services.AddSingleton<CreatedDateUpdatedDateInterceptor>();
        services.AddSingleton<DomainEventsInterceptor>();
        services.RegisterMailing();   
        
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
    
    private static void AddDomainsEventInterceptor(this DbContextOptionsBuilder dbContextOptionsBuilder,
        IServiceProvider serviceProvider)
    {
        var interceptor = serviceProvider.GetService<DomainEventsInterceptor>();

        if (interceptor != null)
        {
            dbContextOptionsBuilder.AddInterceptors(interceptor);
        }
    }

    private static void RegisterMailing(this IServiceCollection serviceCollection)
    {
        // Use a local test email server - configured in Aspire
        // See: https://ardalis.com/configuring-a-local-test-email-server/
        // serviceCollection.AddScoped<IEmailSender, MimeKitEmailSender>();

        // Otherwise use this:
        serviceCollection.AddScoped<IEmailSender, FakeEmailSender>();
    }
}
