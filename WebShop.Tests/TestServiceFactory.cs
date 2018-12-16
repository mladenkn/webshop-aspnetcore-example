using System;
using Microsoft.EntityFrameworkCore;
using WebShop.DataAccess;
using WebShop.Infrastructure.DataAccess;

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
    }
}
