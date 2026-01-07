using Microsoft.EntityFrameworkCore;

namespace CookBook.Clean.Infrastructure.Factories;

public class DbContextSqLiteFactory : IDbContextFactory<CookBookDbContext>
{
    private readonly bool _seedTestingData;
    private readonly DbContextOptionsBuilder<CookBookDbContext> _contextOptionsBuilder = new();

    public DbContextSqLiteFactory(string databaseName, bool seedTestingData = false)
    {
        _seedTestingData = seedTestingData;

        _contextOptionsBuilder.UseSqlite($"Data Source={databaseName};Cache=Shared");

        ////Enable in case you want to see tests details, enabled may cause some inconsistencies in tests
        //_contextOptionsBuilder.EnableSensitiveDataLogging();
        //_contextOptionsBuilder.LogTo(Console.WriteLine);
    }

    public CookBookDbContext CreateDbContext()
    {
        return new CookBookDbContext(_contextOptionsBuilder.Options);
    }
}
