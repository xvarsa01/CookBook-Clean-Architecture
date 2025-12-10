using CookBook.Clean.Infrastructure;
using CookBook.Clean.UnitTests.TestDoubles;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CookBook.Clean.UnitTests.UseCases;

public class EfRepositoryTests
{
    private class TestDbContext : DbContext
    {
        public TestDbContext(DbContextOptions<TestDbContext> options) : base(options) { }
        public DbSet<TestEntity> TestEntities { get; set; } = null!;
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsEntityOrNull()
    {
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        await using (var context = new TestDbContext(options))
        {
            var entityId = Guid.NewGuid();
            var entity = new TestEntity { Id = entityId, Name = "Test" };
            context.Set<TestEntity>().Add(entity);
            await context.SaveChangesAsync();

            var repo = new EfRepository<TestEntity>(context);
            var found = await repo.GetByIdAsync(entityId);
            Assert.Equal(entity.Id, found?.Id);

            var notFound = await repo.GetByIdAsync(Guid.NewGuid());
            Assert.Null(notFound);
        }
    }
}
