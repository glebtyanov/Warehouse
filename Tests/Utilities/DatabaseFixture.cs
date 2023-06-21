using DAL.Context;
using Microsoft.EntityFrameworkCore;

namespace Tests.Utilities
{
    public class DatabaseFixture : IDisposable
    {
        private readonly DbContextOptions<WarehouseContext> options;

        public DatabaseFixture()
        {
            // Create options for in-memory database
            options = new DbContextOptionsBuilder<WarehouseContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
        }

        public WarehouseContext CreateContext()
        {
            return new WarehouseContext(options);
        }

        public void Dispose()
        {
            using (var dbContext = CreateContext())
            {
                dbContext.Database.EnsureDeleted();
            }
        }

        public void ResetDatabase()
        {
            using (var dbContext = CreateContext())
            {
                dbContext.Database.EnsureDeleted();
                dbContext.Database.EnsureCreated();
            }
        }
    }
}