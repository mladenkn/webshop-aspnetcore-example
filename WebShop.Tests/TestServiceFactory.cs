using System;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebShop.DataAccess;

namespace WebShop.Tests
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
