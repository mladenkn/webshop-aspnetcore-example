using System;
using Microsoft.EntityFrameworkCore;
using WebShop.DataAccess;

namespace WebShop.Tests
{
    internal static class TestServiceFactory
    {
        internal static WebShopDbContext InMemoryDatabase()
        {
            var dbOptions = new DbContextOptionsBuilder().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var db = new WebShopDbContext(dbOptions);
            return db;
        }

        internal static WebShopDbContext Database()
        {
            var options = new DbContextOptionsBuilder<WebShopDbContext>()
                .UseSqlite("DataSource=:memory:")
                .Options;

            var context = new WebShopDbContext(options);
            context.Database.OpenConnection();
            context.Database.EnsureCreated();
            return context;
        }
    }
}
