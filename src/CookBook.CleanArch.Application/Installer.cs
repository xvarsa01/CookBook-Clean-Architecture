using CookBook.CleanArch.Application.Behaviors;
using Microsoft.Extensions.DependencyInjection;

namespace CookBook.CleanArch.Application;

public static class Installer
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssembly(typeof(Installer).Assembly);
            options.AddOpenBehavior(typeof(UnitOfWorkBehavior<,>));
        });
        return services;
    }
}
