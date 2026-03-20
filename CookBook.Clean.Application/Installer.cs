using CookBook.Clean.Application.Mappers;
using Microsoft.Extensions.DependencyInjection;

namespace CookBook.Clean.Application;

public static class Installer
{
    public static IServiceCollection AddUseCasesServices(this IServiceCollection services)
    {
        services.AddSingleton<IRecipeMapper, ManualRecipeMapper>();
        services.AddSingleton<IIngredientMapper, ManualIngredientMapper>();
        
        services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssembly(typeof(Installer).Assembly);
        });
        return services;
    }
}
