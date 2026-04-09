using Microsoft.EntityFrameworkCore.Design;

namespace CookBook.CleanArch.Infrastructure.Factories;

/// <summary>
///     EF Core CLI migration generation uses this DbContext to create model and migration
/// </summary>
public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<CookBookDbContext>
{
    private readonly DbContextSqLiteFactory _dbContextSqLiteFactory = new("cookbook.db", true);

    public CookBookDbContext CreateDbContext(string[] args)
    {
        return _dbContextSqLiteFactory.CreateDbContext();
    }
}
