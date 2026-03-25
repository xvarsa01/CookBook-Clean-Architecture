using CookBook.CleanArch.UnitTests.TestDoubles;
using Microsoft.EntityFrameworkCore;

namespace CookBook.CleanArch.UnitTests.UseCases;

public class EfRepositoryTests
{
    private class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options) { }
        public DbSet<TestBase> TestEntities { get; set; } = null!;
    }
}
