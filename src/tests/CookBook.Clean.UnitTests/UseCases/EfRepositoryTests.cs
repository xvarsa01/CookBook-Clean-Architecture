using CookBook.Clean.Infrastructure.Repositories;
using CookBook.Clean.UnitTests.TestDoubles;
using Microsoft.EntityFrameworkCore;

namespace CookBook.Clean.UnitTests.UseCases;

public class EfRepositoryTests
{
    private class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options) { }
        public DbSet<TestBase> TestEntities { get; set; } = null!;
    }
}
