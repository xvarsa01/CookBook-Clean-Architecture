using CookBook.CleanArch.Application.ExternalInterfaces;
using CookBook.CleanArch.Infrastructure;

namespace CookBook.CleanArch.Presentation.WebApi.FunctionalTests.Infrastructure;

internal sealed class WebApiTestDbMigrator(CookBookDbContext dbContext) : IDbMigrator
{
    public void Migrate()
    {
        dbContext.Database.EnsureCreated();
    }
}
