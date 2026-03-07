using CookBook.Clean.UseCases.Mappers;
using Microsoft.Extensions.DependencyInjection;

namespace CookBook.Clean.UseCases;

public static class UseCasesInstaller
{
    public static IServiceCollection AddUseCasesServices(this IServiceCollection services)
    {
        services.AddSingleton<IRecipeMapper, ManualRecipeMapper>();
        services.AddSingleton<IIngredientMapper, ManualIngredientMapper>();
        
        services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssembly(typeof(UseCasesInstaller).Assembly);
        });
        return services;
    }
}
