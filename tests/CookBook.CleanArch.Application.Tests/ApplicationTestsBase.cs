using CookBook.CleanArch.Application.Behaviors;
using CookBook.CleanArch.Application.ExternalInterfaces;
using CookBook.CleanArch.Common.Tests;
using CookBook.CleanArch.Domain.Recipes;
using CookBook.CleanArch.Domain.Recipes.ValueObjects;
using CookBook.CleanArch.Infrastructure;
using CookBook.CleanArch.Infrastructure.Factories;
using CookBook.CleanArch.Infrastructure.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CookBook.CleanArch.Application.Tests;

public class ApplicationTestsBase : IAsyncLifetime
{
    private readonly ServiceProvider _serviceProvider;

    protected ApplicationTestsBase()
    {
        var databaseName = $"{GetType().FullName}_{Guid.NewGuid():N}.db";
        DbContextFactory = new DbContextSqLiteFactory(databaseName);

        var services = new ServiceCollection();
        services.AddDbContext<CookBookDbContext>(options =>
        {
            options.UseSqlite($"Data Source={databaseName}");
        });

        services.AddScoped<ICookBookDbContext>(sp => sp.GetRequiredService<CookBookDbContext>());
        services.AddScoped<DbContext>(sp => sp.GetRequiredService<CookBookDbContext>());

        services.AddScoped(typeof(IRepository<,>), typeof(EfRepository<,>));
        services.AddScoped(typeof(IRepository<Recipe, RecipeId>), typeof(EfRecipeRepository));
        services.AddScoped<IRecipeRepository, EfRecipeRepository>();

        services.AddLogging();
        services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssembly(typeof(Installer).Assembly);
            options.AddOpenBehavior(typeof(UnitOfWorkBehavior<,>));
        });
        
        _serviceProvider = services.BuildServiceProvider();
    }

    protected IDbContextFactory<CookBookDbContext> DbContextFactory { get; }
    protected CookBookDbContext DbContext { get; private set; } = null!;
    public IMediator Mediator { get; private set; } = null!;

    protected IReadOnlyList<Recipe> SeededRecipes { get; private set; } = [];

    protected Recipe GetSeededRecipeByName(string recipeName) =>
        SeededRecipes.Single(r => r.Name.Value == recipeName);

    public async Task InitializeAsync()
    {
        var scope = _serviceProvider.CreateScope();
        var scopedProvider = scope.ServiceProvider;

        DbContext = scopedProvider.GetRequiredService<CookBookDbContext>();
        Mediator = scopedProvider.GetRequiredService<IMediator>();
        
        
        await DbContext.Database.EnsureDeletedAsync();
        await DbContext.Database.EnsureCreatedAsync();

        DbContext.AddRange(IngredientTestSeeds.SeededIngredients);
        DbContext.AddRange(RecipeTestSeeds.SeededRecipes);
        await DbContext.SaveChangesAsync();
        DbContext.ChangeTracker.Clear();

        SeededRecipes = await DbContext.Recipes
            .AsNoTracking()
            .Include(r => r.Ingredients)
            .ThenInclude(ri => ri.Ingredient)
            .ToListAsync();
    }

    public async Task DisposeAsync()
    {
        await DbContext.Database.EnsureDeletedAsync();
        await DbContext.DisposeAsync();
        await _serviceProvider.DisposeAsync();
    }
}
