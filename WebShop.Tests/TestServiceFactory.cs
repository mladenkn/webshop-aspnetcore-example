using System;
using Microsoft.EntityFrameworkCore;
using WebShop.DataAccess;

namespace WebShop.Tests
{
    internal static class TestServiceFactory
    {
        internal static WebShopDbContext Database()
        {
            var dbOptions = new DbContextOptionsBuilder().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var db = new WebShopDbContext(dbOptions);
            return db;
        }
    }
}
