using CookBook.CleanArch.Application.ExternalInterfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CookBook.CleanArch.Infrastructure.Migrator;

public class DbMigrator(IDbContextFactory<CookBookDbContext> dbContextFactory, IOptions<DbOptions> options)
    : IDbMigrator
{
    public void Migrate()
    {
        using CookBookDbContext dbContext = dbContextFactory.CreateDbContext();

        if(options.Value.RecreateDatabaseEachTime)
        {
            dbContext.Database.EnsureDeleted();
        }

        // Ensures that database is created applying the latest state
        // Application of migration later on may fail
        // If you want to use migrations, you should create database by calling  dbContext.Database.Migrate() instead
        dbContext.Database.EnsureCreated();
    }
}
