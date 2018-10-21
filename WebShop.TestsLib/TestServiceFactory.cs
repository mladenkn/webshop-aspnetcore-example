using System;
using Microsoft.EntityFrameworkCore;
using WebShop.Infrastructure;

namespace WebShop.TestsLib
{
    public static class TestServiceFactory
    {
        public static WebShopDbContext InMemoryDatabase()
        {
            var dbOptions = new DbContextOptionsBuilder().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var db = new WebShopDbContext(dbOptions);
            return db;
        }
    }
}
